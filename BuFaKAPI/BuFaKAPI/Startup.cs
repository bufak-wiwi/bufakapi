// <copyright file="Startup.cs" company="BuFaKWiSo">
// Copyright (c) BuFaKWiSo. All rights reserved.
// </copyright>

namespace BuFaKAPI
{
    using System;
    using System.IO;
    using System.Reflection;
    using BuFaKAPI.Models;
    using FirebaseAdmin;
    using Google.Apis.Auth.OAuth2;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            this._env = env;
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<AppSettings>(this.Configuration.GetSection("AppSettings"));

            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "BuFaK API",
                    Description = "An API for the BuFaK WiSo",
                    TermsOfService = "None",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("Services/bufak-wiso-e960b1a417d5.json"),
            });

            if (this._env.IsDevelopment())
            {
                var connection = KeyManager.getDatabaseDevString();

                services.AddDbContext<MyContext>(options => options.UseMySql(connection));
            }
            else
            {
                var connection = KeyManager.getDatabaseString();
                services.AddDbContext<MyContext>(options => options.UseMySql(connection));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
