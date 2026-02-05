
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ComputerSeekho.API.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Add Data Protection for OAuth state encryption
            services.AddDataProtection()
                .SetApplicationName("ComputerSeekho")
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "DataProtectionKeys")));

            var jwtSecret = configuration["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("JWT Secret not configured");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "ComputerSeekho.Auth";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.Lax;  // Changed to Lax for localhost
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Allow HTTP in dev
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // Mark as essential for GDPR
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
                options.CallbackPath = "/signin-google";
                options.SaveTokens = true;

                // Request email and profile
                options.Scope.Add("email");
                options.Scope.Add("profile");

                // CRITICAL for OAuth state cookie - match main cookie settings
                options.CorrelationCookie.Name = ".AspNetCore.Correlation.Google";
                options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.CorrelationCookie.HttpOnly = true;
                options.CorrelationCookie.IsEssential = true;

                // Tell Google to use Cookie for sign-in
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            return services;
        }
    }
}
