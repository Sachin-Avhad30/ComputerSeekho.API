using ComputerSeekho.API.Extensions;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// ADD SERVICES TO CONTAINER
// ========================================

ExcelPackage.License.SetNonCommercialPersonal("ComputerSeekho");


// Application Services (Database, Repositories, Services)
builder.Services.AddApplicationServices(builder.Configuration);

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// CORS Policy
builder.Services.AddCorsPolicy(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI with File Upload Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();  // ✅ Use custom Swagger config

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

// ========================================
// CONFIGURE HTTP REQUEST PIPELINE
// ========================================

// Development tools

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ComputerSeekho API v1");
        c.RoutePrefix = "swagger";  // Access at /swagger
    });
}

// Serve static files (uploaded images)
app.UseStaticFiles();

// HTTPS Redirection
app.UseHttpsRedirection();

// ⚠️ ORDER MATTERS! CORS must come BEFORE Authentication


// Authentication & Authorization
app.UseAuthentication();

app.UseCors("AllowFrontend");

app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();