using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspNetCoreRateLimit;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using eCommerceApp.Entities.DTO;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.Models.Identity;
using eCommerceApp.LoggerService;
using eCommerceApp.Repository;
using eCommerceApp.Repository.DataShaping;
using eCommerceApp.Server.ActionFilters;
using eCommerceApp.Server.Authentication;
using eCommerceApp.Server.Utility;
using eCommerceApp.Service;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace eCommerceApp.Server.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Configure CORS
        /// </summary>
        /// <param name="service"></param>
        public static void ConfigureCors(this IServiceCollection service)
        => service.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                                                                .AllowAnyMethod()
                                                                .AllowAnyHeader());
        });

        /// <summary>
        /// Configure swagger
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(options =>
        {
            // Configure versioning
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "APIs",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Nhan Nguyen",
                    Email = "nguyenlinhthanhnhan@outlook.com",
                    Url = new Uri("https://www.facebook.com/nhannguyen.dev/")
                }
            });

            // Adding Authorization Support
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                {new OpenApiSecurityScheme{
                    Reference=new OpenApiReference{
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    },
                    Name="Bearer",
                },
                new List<string>()}
            });
        });

        /// <summary>
        /// Configure LoggerManager
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLoggerService(this IServiceCollection services)
        => services.AddScoped<ILoggerManager, LoggerManager>();

        /// <summary>
        /// Configure database connecting
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSQLDataContext(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<RepositoryDataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"),
                                 x => x.MigrationsAssembly("eCommerceApp.Server"));
        });

        /// <summary>
        /// Configure Identity
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(x =>
            {
                x.Password.RequireDigit = true;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequiredLength = 8;
                x.Password.RequireNonAlphanumeric = false;
                x.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<RepositoryDataContext>().AddDefaultTokenProviders();
        }

        public static void DIConfigureRepositoryManager(this IServiceCollection services)
        => services.AddScoped<IRepositoryManager, RepositoryManager>();

        /// <summary>
        /// DI reg for CategoryRepository
        /// </summary>
        /// <param name="services"></param>
        public static void DIConfigureCategoryRepository(this IServiceCollection services)
        => services.AddScoped<ICategoryRepository, CategoryRepository>();

        public static void DIConfigureCategoryService(this IServiceCollection services)
        => services.AddScoped<ICategoryService, CategoryService>();

        /// <summary>
        /// DI reg for ValidateCategoryExistAttribute
        /// </summary>
        /// <param name="services"></param>
        public static void DIConfigureValidateCategoryExistAttribute(this IServiceCollection services)
        => services.AddScoped<ValidateCategoryExistsAttribute>();

        /// <summary>
        /// DI reg for ValidationFilterAttribute
        /// </summary>
        /// <param name="services"></param>
        public static void DIConfigureValidationFilterAttribute(this IServiceCollection services)
        => services.AddScoped<ValidationFilterAttribute>();

        /// <summary>
        /// DI reg for DataShaper
        /// </summary>
        /// <param name="services"></param>
        public static void DIConfigureDataShaper(this IServiceCollection services)
        => services.AddScoped(typeof(IDataShaper<>), typeof(DataShaper<>));

        /// <summary>
        /// Add custom media for support HATEOAS
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.nhannguyen.hateoas+json");
                }

                var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.nhannguyen.hateoas+xml");
                }
            });
        }

        /// <summary>
        /// DI reg for ValidateMediaTypeAttribute
        /// </summary>
        /// <param name="services"></param>
        public static void DIConfigureValidateMediaTypeAttribute(this IServiceCollection services)
        => services.AddScoped<ValidateMediaTypeAttribute>();

        /// <summary>
        /// DI reg for CategoryLinks, implement HATEOAS
        /// </summary>
        /// <param name="services"></param>
        public static void DIConfigureCategoryLinks(this IServiceCollection services)
        => services.AddScoped<CategoryLinks>();

        /// <summary>
        /// Configure Versioning
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureVersioning(this IServiceCollection services)
        => services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        /// <summary>
        /// Add cache-store
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureResponseCaching(this IServiceCollection services)
        => services.AddResponseCaching();

        /// <summary>
        /// Configure HTTP Cache
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        => services.AddHttpCacheHeaders(expirationOptions =>
        {
            expirationOptions.MaxAge = 180;
            expirationOptions.CacheLocation = CacheLocation.Private;
        }, validationOptions =>
        {
            validationOptions.MustRevalidate = true;
        });

        /// <summary>
        /// Configure limiting rule
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>{
                new RateLimitRule{
                    Endpoint="*",
                    Limit=20,
                    Period="1m"
                }
            };
            services.Configure<IpRateLimitOptions>(opt => opt.GeneralRules = rateLimitRules);

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        /// <summary>
        /// DI ref for AuthenticationManager
        /// </summary>
        /// <param name="services"></param>
        public static void DIConfigureAuthenticationManager(this IServiceCollection services)
        => services.AddScoped<IAuthenticationManager, AuthenticationManager>();

        /// <summary>
        /// Configure JWT Bearer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }
}