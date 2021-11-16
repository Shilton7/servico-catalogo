using CatalogoAPI.Configuration;
using CatalogoAPI.Configuration.DI;
using CatalogoAPI.Configuration.IdentityServer;
using CatalogoAPI.Configuration.Swagger;
using CatalogoAPI.Context;
using CatalogoAPI.Filters;
using CatalogoAPI.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CatalogoAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string mySqlConnectionStr = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                     options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
            );

            services.AddIdentityConfiguration(_configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddApiConfig();

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                //Auth Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddScoped<ApiLoggingFilter>();

            services.ResolveDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                options =>
                {
                    foreach (ApiVersionDescription apiVersionDescription in provider.ApiVersionDescriptions.OrderByDescending(a => a.ApiVersion))
                    {
                        string isDeprecated = apiVersionDescription.IsDeprecated ? " (DEPRECATED)" : string.Empty;
                        options.SwaggerEndpoint($"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                            $"{apiVersionDescription.GroupName.ToUpperInvariant()}{isDeprecated}");
                    }
                });
            }

            loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfig
            {
                LogLevel = LogLevel.Information
            }, _configuration));

            app.UseApiConfig(env);

        }
    }
}
