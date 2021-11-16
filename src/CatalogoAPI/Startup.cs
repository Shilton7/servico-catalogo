using CatalogoAPI.Configuration;
using CatalogoAPI.Configuration.DI;
using CatalogoAPI.Configuration.IdentityServer;
using CatalogoAPI.Configuration.Swagger;
using CatalogoAPI.Context;
using CatalogoAPI.Filters;
using CatalogoAPI.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;

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

            services.AddSwaggerConfig();

            services.AddScoped<ApiLoggingFilter>();

            services.ResolveDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfig
            {
                LogLevel = LogLevel.Information
            }, _configuration));

            app.UseApiConfig(env);

            app.UseSwaggerConfig(provider);

        }
    }
}
