using CatalogoAPI.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CatalogoAPI.Configuration.IdentityServer
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
                                                        IConfiguration configuration)
        {
            string mySqlConnectionStr = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                     options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
            );

            // Adicionando o Identity na aplicação
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            // JWT
            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidAudience = configuration["AppSettingsJWT:Audience"],
                     ValidIssuer = configuration["AppSettingsJWT:Issuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(configuration["AppSettingsJWT:Secret"]))
                 });

            return services;
        }
    }
}