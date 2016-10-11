using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filelocker.Domain.Interfaces;
using Filelocker.FileSystemProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Filelocker.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddScoped<IFileStorageProvider>(p => new FileSystemStorageProvider(@"C:\Temp\filelocker"));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IFileStorageProvider fileStorageProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.Use(async (context, next) =>
            {
                if (!IsMultipartContentType(context.Request.ContentType))
                {
                    await next();
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

                        using (var stream = fileStorageProvider.GetStream(fileName))
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

            });

            app.UseMvc();
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
