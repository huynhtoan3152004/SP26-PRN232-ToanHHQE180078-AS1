using System.Text;
using HuynhHuuToan__SE1856_A01_BE.Middleware;
using HuynhHuuToan__SE1856_A01_Repository.Models.Data;
using HuynhHuuToan__SE1856_A01_Repository.UnitOfWork;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// === DATABASE ===
builder.Services.AddDbContext<FUNewsManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// === REPOSITORY LAYER (Unit of Work) ===
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// === SERVICE LAYER ===
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<IAuthService, AuthService>(); // JWT Auth Service

// === JWT AUTHENTICATION (Requirement 7) ===
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// === SWAGGER/OpenAPI (Requirement 10) ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FU News Management API",
        Version = "v1",
        Description = "PRN232 Assignment 01 - RESTful API with JWT Authentication"
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by space and JWT token.\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR..."
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// === GLOBAL EXCEPTION HANDLER (Requirement 8) ===
// Phải đặt trước các middleware khác để catch tất cả exceptions
app.UseGlobalExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Remove HTTPS redirection for Docker environment
if (!app.Environment.EnvironmentName.Equals("Docker"))
{
    app.UseHttpsRedirection();
}

// === AUTHENTICATION & AUTHORIZATION (Requirement 7) ===
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Redirect root to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
