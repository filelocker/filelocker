using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Filelocker.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Filelocker.Domain;
using System.Security.Claims;

namespace Filelocker.Api.Middleware
{
    public class UploadMiddleware
    {
        private string _route = "/api/files";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IFileStorageProvider _fileStorageProvider;
        private readonly IUnitOfWork _unitOfWork;

        public UploadMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IFileStorageProvider fileStorageProvider, IUnitOfWork unitOfWork)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<UploadMiddleware>();
            _fileStorageProvider = fileStorageProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToLower() == "post"  && context.Request.Path.HasValue && context.Request.Path.Value == _route)
            {
                if (context.User.Identity.IsAuthenticated == false)
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("Must be authenticated to upload files.");
                    return;
                }
                var identity = (ClaimsIdentity)context.User.Identity;
                var userId = int.Parse(identity.Claims.Where(c => c.Type == "sub")
                   .Select(c => c.Value).SingleOrDefault());
                _logger.LogInformation("Handling file upload: " + context.Request.Path);
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
                        var fileName = GetFileName(section.ContentDisposition);

                        using (var stream = _fileStorageProvider.GetStream(fileName))
                        {
                            var bytesRead = 0;
                            do
                            {
                                bytesRead = await section.Body.ReadAsync(fileBuffer, 0, fileBuffer.Length);

                                //TODO: Encrypt
                                await stream.WriteAsync(fileBuffer, 0, bytesRead);
                                //TODO: Update a SignalR Hub for progress tracking

                            } while (bytesRead > 0);
                        }
                        _unitOfWork.FileRepository.Add(new FilelockerFile()
                        {
                            UserId = userId,
                            Name = fileName
                        });
                    }
                    else // A form KVP
                    {
                        var kvp = await GetKeyValuePairFromFormDataAsync(section);
                        bodyValues.Add(kvp.Key, kvp.Value);
                    }

                    section = await reader.ReadNextSectionAsync();
                }
                
                await _unitOfWork.CommitAsync();
                // Write response
                await context.Response.WriteAsync("Done.");
                
                _logger.LogInformation("Finished file upload.");
                return;
            }
            await _next.Invoke(context);
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
