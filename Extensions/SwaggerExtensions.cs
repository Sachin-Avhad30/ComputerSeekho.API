using Microsoft.OpenApi.Models;

namespace ComputerSeekho.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ComputerSeekho API",
                    Version = "v1",
                    Description = "API for ComputerSeekho - Staff Authentication & Management",
                    Contact = new OpenApiContact
                    {
                        Name = "ComputerSeekho Team",
                        Email = "support@computerseekho.com"
                    }
                });

                // Add JWT Bearer Authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your JWT token. Example: Bearer eyJhbGc..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                // ✅ IMPORTANT: Enable support for multipart/form-data file uploads
                options.MapType<IFormFile>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                });
            });

            return services;
        }
    }
}