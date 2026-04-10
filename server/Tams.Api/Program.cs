using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Tams.Api.Repos;
// Services will be added here as needed
// using Tams.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// Database Connection
// We register IDbConnection as a scoped service, so that each HTTP request gets its own connection instance.
// The connection string is read from the configuration (appsettings.json or environment variables).
// This allows repositories to receive an IDbConnection via constructor injection.
// ==========================================
builder.Services.AddScoped<IDbConnection>(db =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

// ==========================================
// Repositories
// ==========================================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IWarrantyRepository, WarrantyRepository>();
builder.Services.AddScoped<IPricingRepository, PricingRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

// ==========================================
// Services - Uncomment services as they are implemented
// ==========================================
// builder.Services.AddScoped<IItemService, ItemService>();
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<IPricingService, PricingService>();
// builder.Services.AddScoped<IInventoryService, InventoryService>();


// ==========================================
// Authentication - JWT Bearer
// ==========================================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

builder.Services.AddAuthorization();

// ==========================================
// CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactSpa", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // React development server URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ==========================================
// HTTP Client
// For external API calls (e.g., pricing services), we can register HttpClient here.
// ==========================================
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ==========================================
var app = builder.Build();

// ==========================================
// Exception Handling
// ==========================================
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        if (feature != null)
        {
            logger.LogError(feature.Error, "An unhandled exception occurred.");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
    });
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("ReactSpa");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();