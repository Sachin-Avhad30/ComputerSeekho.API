//using ComputerSeekho.API.Extensions;
//using OfficeOpenXml;

//var builder = WebApplication.CreateBuilder(args);

//// ========================================
//// ADD SERVICES TO CONTAINER
//// ========================================

//ExcelPackage.License.SetNonCommercialPersonal("ComputerSeekho");


//// Application Services (Database, Repositories, Services)
//builder.Services.AddApplicationServices(builder.Configuration);

//// JWT Authentication
//builder.Services.AddJwtAuthentication(builder.Configuration);

//// CORS Policy
//builder.Services.AddCorsPolicy(builder.Configuration);

//// Controllers
//builder.Services.AddControllers();

//// Swagger/OpenAPI with File Upload Support
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerConfiguration();  // ✅ Use custom Swagger config

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend", policy =>
//    {
//        policy
//            .AllowAnyOrigin()     
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

//var app = builder.Build();

//// ========================================
//// CONFIGURE HTTP REQUEST PIPELINE
//// ========================================

//// Development tools

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ComputerSeekho API v1");
//        c.RoutePrefix = "swagger";  // Access at /swagger
//    });
//}

//// Serve static files (uploaded images)
//app.UseStaticFiles();

//// HTTPS Redirection
//app.UseHttpsRedirection();

//// ⚠️ ORDER MATTERS! CORS must come BEFORE Authentication


//// Authentication & Authorization
//app.UseAuthentication();

//app.UseCors("AllowFrontend");

//app.UseAuthorization();

//// Map Controllers
//app.MapControllers();

//app.Run();
using ComputerSeekho.API.Extensions;
using ComputerSeekho.API.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);


ExcelPackage.License.SetNonCommercialPersonal("ComputerSeekho");



builder.Services.AddApplicationServices(builder.Configuration);


builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddCorsPolicy(builder.Configuration);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.CheckConsentNeeded = context => false; // Disable consent check for OAuth
    options.OnAppendCookie = cookieContext =>
        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
    options.OnDeleteCookie = cookieContext =>
        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ComputerSeekho API v1");
        c.RoutePrefix = "swagger";
    });
}


app.UseStaticFiles();

// Only enforce HTTPS in production to allow HTTP frontend (localhost) to work with OAuth
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Cookie policy must come before authentication
app.UseCookiePolicy();

//app.UseMiddleware<GlobalException>();


app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


void CheckSameSite(HttpContext httpContext, CookieOptions options)
{
    if (options.SameSite == SameSiteMode.None)
    {
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        if (DisallowsSameSiteNone(userAgent))
        {
            options.SameSite = SameSiteMode.Unspecified;
        }
    }
}

bool DisallowsSameSiteNone(string userAgent)
{
    // Checks if the browser is known to have issues with SameSite=None
    if (string.IsNullOrEmpty(userAgent)) return false;

    // Cover iOS 12 and certain Mac Safari versions
    return userAgent.Contains("CPU iPhone OS 12") ||
           userAgent.Contains("iPad; CPU OS 12") ||
           (userAgent.Contains("Safari") &&
            userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
            userAgent.Contains("Version/12."));
}