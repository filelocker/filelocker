using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

using Filelocker.Domain;
using Filelocker.Domain.Interfaces;
using Filelocker.Domain.Constants;
using Filelocker.Api.Extensions;

namespace Filelocker.Api.Middleware
{
    public class UploadMiddleware
    {
        private string _route = "/api/files";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IFileService _fileService;

        public UploadMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<UploadMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToLower() == "put" && context.Request.Path.HasValue && context.Request.Path.Value == _route)
            {
                if (context.User.Identity.IsAuthenticated == false)
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("Must be authenticated to upload files.");
                    return;
                }

                if (!IsMultipartContentType(context.Request.ContentType))
                {
                    await _next.Invoke(context);
                    return;
                }
                var bodyValues = new Dictionary<string, string>();
                // Copy the request stream to the memory stream.
                var boundary = GetBoundary(context.Request.ContentType);
                var reader = new MultipartReader(boundary, context.Request.Body);
                var section = await reader.ReadNextSectionAsync();

                while (section != null)
                {
                    // process each image
                    const int chunkSize = 1024;
                    var fileBuffer = new byte[chunkSize];
                    if (IsFileSection(section.ContentDisposition))
                    {
                        FilelockerFile filelockerFile = null;
                        try
                        {
                            var fileName = GetFileName(section.ContentDisposition);
                            filelockerFile = new FilelockerFile()
                            {
                                UserId = context.User.Identity.GetUserId(),
                                Name = fileName,
                                EncryptionKey = Guid.NewGuid().ToString(),
                                EncryptionSalt = Guid.NewGuid()
                            };
                            await _fileService.CreateOrUpdateFileAsync(filelockerFile);

                            using (var fs = _fileService.GetWriteStream(filelockerFile.Id.ToString()))
                            using (var encryptor = Aes.Create())
                            {
                                var pdb = new Rfc2898DeriveBytes(filelockerFile.EncryptionKey, filelockerFile.EncryptionSalt.ToByteArray());
                                encryptor.Key = pdb.GetBytes(32);
                                encryptor.IV = pdb.GetBytes(16);

                                using (var cs = new CryptoStream(fs, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                                {
                                    var bytesRead = 0;
                                    do
                                    {
                                        bytesRead = await section.Body.ReadAsync(fileBuffer, 0, fileBuffer.Length);
                                        await cs.WriteAsync(fileBuffer, 0, fileBuffer.Length);
                                    } while (bytesRead > 0);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning((int)FilelockerEventTypes.FileUpload, ex, "File upload failed");
                            if (filelockerFile != null)
                            {
                                await _fileService.DeleteFileAsync(filelockerFile.Id);
                            }
                            throw;
                        }
                    }
                    else // A form KVP
                    {
                        var kvp = await GetKeyValuePairFromFormDataAsync(section);
                        bodyValues.Add(kvp.Key, kvp.Value);
                    }

                    section = await reader.ReadNextSectionAsync();
                }

                // Write response
                await context.Response.WriteAsync("Done.");

                _logger.LogInformation((int)FilelockerEventTypes.FileUpload, "File uploaded successfully.");
                return;
            }
            await _next.Invoke(context);
            return;
        }

        private static bool IsMultipartContentType(string contentType)
        {
            return
                !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string GetBoundary(string contentType)
        {
            var elements = contentType.Split(' ');
            var element = elements.First(entry => entry.StartsWith("boundary="));
            var boundary = element.Substring("boundary=".Length);
            // Remove quotes
            if (boundary.Length >= 2 && boundary[0] == '"' &&
                boundary[boundary.Length - 1] == '"')
            {
                boundary = boundary.Substring(1, boundary.Length - 2);
            }
            return boundary;
        }

        private async Task<KeyValuePair<string, string>> GetKeyValuePairFromFormDataAsync(MultipartSection section)
        {
            string key = string.Empty;
            string value = string.Empty;

            var parts = section.ContentDisposition.Split(';');
            if (parts.SingleOrDefault(part => part.Contains("name")) != null)
            {
                key = parts[1].Split('=')[1].Replace("\\", "").Replace("\"", "");
                using (var streamReader = new StreamReader(section.Body))
                {
                    value = await streamReader.ReadToEndAsync();
                }
            }

            return new KeyValuePair<string, string>(key, value);
        }

        private bool IsFileSection(string contentDisposition)
        {
            return contentDisposition.Split(';').SingleOrDefault(part => part.Contains("filename")) != null;
        }

        private string GetFileName(string contentDisposition)
        {
            return contentDisposition
                .Split(';')
                .SingleOrDefault(part => part.Contains("filename"))
                .Split('=')
                .Last()
                .Trim('"');
        }
    }
}
