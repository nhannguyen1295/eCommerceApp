using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using eCommerceApp.Server.Extensions;
using NLog;
using System.IO;
using eCommerceApp.Contract;
using AutoMapper;
using AspNetCoreRateLimit;
using eCommerceApp.Server.ActionFilters;
using Microsoft.AspNetCore.Http;

namespace eCommerceApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.AddAutoMapper(typeof(Startup));
            services.ConfigureVersioning();
            services.ConfigureResponseCaching();
            services.ConfigureHttpCacheHeaders();
            services.AddMemoryCache();

            services.ConfigureRateLimitingOptions();
            services.AddHttpContextAccessor();

            services.DIConfigureRepositoryManager();
            services.DIConfigureCategoryRepository();
            services.DIConfigureCategoryService();
            services.DIConfigureValidateCategoryExistAttribute();
            services.DIConfigureValidationFilterAttribute();
            services.DIConfigureDataShaper();
            services.DIConfigureCategoryLinks();
            services.DIConfigureAuthenticationManager();
            services.DIConfigureProductRepository();
            services.DIConfigureProductService();
            services.DIConfigureProductCategoryRepository();
            services.DIConfigureValidateProductCategoryExistsAttribute();
            services.DIConfigureValidateProductExistAttribute();

            services.ConfigureSQLDataContext(Configuration);
            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);
            services.ConfigureLoggerService();


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(configure =>
            {
                configure.Filters.Add(typeof(ValidateMediaTypeAttribute));
                configure.RespectBrowserAcceptHeader = true;
                configure.ReturnHttpNotAcceptable = true; // HTTP 406 if client required non-support media types
            }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

            services.AddCustomMediaTypes(); // !!!Always put it under AddControllers to avoid ERR 

            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIs v1"));
            }

            app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
            });

            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
