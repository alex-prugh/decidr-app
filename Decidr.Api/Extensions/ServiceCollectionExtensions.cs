using Decidr.Infrastructure;
using Decidr.Operations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Decidr.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDecidrServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        // Fluent validation.
        services.AddFluentValidationClientsideAdapters().AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();

        // Add Swagger services
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Decidr API", Version = "1.0.0" });

            // Add JWT Bearer auth definition
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: `Bearer eyJhbGciOi...`"
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
                    Array.Empty<string>()
                }
            });
        });

        var jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("JWT Token");

        // API Auth
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // TODO: Make it more sophisticated
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        services.AddAuthorization();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                // TODO: Put client connection string in app settings.
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Other project services.
        services.AddInfrastructure(configuration);
        services.AddOperations();

        return services;
    }
}