﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Filelocker.Api.Middleware;
using Filelocker.Api.Models;
using Filelocker.Domain.Interfaces;
using Filelocker.FileSystemProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Model;

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
            services.AddDeveloperIdentityServer()
                .AddInMemoryScopes(IdentityConfig.GetScopes())
                .AddInMemoryClients(IdentityConfig.GetClients());
            //var source = System.IO.File.ReadAllText("MyCertificate.b64cert");
            //var certBytes = Convert.FromBase64String(source);
            //var certificate = new X509Certificate2(certBytes, "password");

            //var builder = services.AddIdentityServer(options =>
            //{
            //    options. = certificate;
            //    options.ss = false; // should be true
            //});

            //builder.AddInMemoryClients(Clients.Get());
            //builder.AddInMemoryScopes(Scopes.Get());
            //builder.AddInMemoryUsers(Users.Get());

            // Add framework services.
            services.AddScoped<IFileStorageProvider>(p => new FileSystemStorageProvider(@"C:\Temp\filelocker"));
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Geo Search API",
                    Description = "A simple api to search using geo location in Elasticsearch",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
            });
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                ScopeName = "filelockerApi",

                RequireHttpsMetadata = false
            });
            app.UseMiddleware<UploadMiddleware>();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseIdentityServer();
        }


    }
}
