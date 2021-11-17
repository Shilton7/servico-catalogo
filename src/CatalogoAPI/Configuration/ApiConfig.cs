using CatalogoAPI.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace CatalogoAPI.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling
                 = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

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

            string mySqlConnectionStr = configuration.GetConnectionString("DefaultConnection");

            services.AddHealthChecks()
                    .AddMySql(mySqlConnectionStr, name: "Instancia MySQL Server")
                    .AddCheck("Table Produtos", new MySQLHealthCheck(mySqlConnectionStr));

            services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(5);
                options.MaximumHistoryEntriesPerEndpoint(10);
                options.AddHealthCheckEndpoint("HealthCheck - Infraestrutura", "/health-data");
            }).AddInMemoryStorage();

            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(opt => opt.AllowAnyMethod()
                                          .AllowAnyOrigin()
                                          .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/health-data", new HealthCheckOptions
            {
                Predicate = p => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => { options.UIPath = "/health-dashboard"; });

            return app;
        }
    }

}