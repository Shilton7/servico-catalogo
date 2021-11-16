using CatalogoAPI.Configuration.Swagger;
using CatalogoAPI.Context;
using CatalogoAPI.Repository;
using CatalogoAPI.Repository.Async.Interfaces;
using CatalogoAPI.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CatalogoAPI.Configuration.DI
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }

    }
}