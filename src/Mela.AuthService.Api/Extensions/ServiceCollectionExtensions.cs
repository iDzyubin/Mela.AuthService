using System.Reflection;
using FluentValidation.AspNetCore;
using Mela.AuthService.Api.Contexts;
using Mela.AuthService.Api.Options;
using Mela.AuthService.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Mela.AuthService.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services, WebApplicationBuilder builder) =>
        services
            .AddDatabase(builder)
            .AddAutomapper()
            .AddValidation()
            .AddJwtAuthentication()
            .AddSwaggerGenerator()
            .AddMailService();
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, WebApplicationBuilder builder) => services
        .AddDbContext<UserContext>(options => options.UseNpgsql(
            builder.Environment.IsDevelopment()
                ? builder.Configuration.GetConnectionString("UserDbConnectionString")
                : Environment.GetEnvironmentVariable("UserDbConnectionString") ?? string.Empty));
    
    private static IServiceCollection AddAutomapper(this IServiceCollection services) => services
        .AddAutoMapper(expression => expression.AddMaps(TargetAssembly));
    
    private static IServiceCollection AddValidation(this IServiceCollection services) => services
        .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssembly(TargetAssembly));

    private static IServiceCollection AddMailService(this IServiceCollection services) => services
        .AddScoped<IMailService, MailService>();
    
    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = AuthOptions.Audience,

                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
        
                ValidateLifetime = true,
            };
        });
        services.AddAuthorization();
        return services;
    }
    
    private static IServiceCollection AddSwaggerGenerator(this IServiceCollection services) => services
        .AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
    
    private static readonly Assembly TargetAssembly = Assembly.GetExecutingAssembly();
}