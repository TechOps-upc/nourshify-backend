using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nourishify.API.Security.Authorization.Handlers.Implementations;
using Nourishify.API.Security.Authorization.Handlers.Interfaces;
using Nourishify.API.Security.Authorization.Middleware;
using Nourishify.API.Security.Authorization.Settings;
using Nourishify.API.Security.Domain.Repositories;
using Nourishify.API.Security.Domain.Services;
using Nourishify.API.Security.Mapping;
using Nourishify.API.Security.Persistence.Repositories;
using Nourishify.API.Security.Services;
using Nourishify.API.Shared.Domain.Repositories;
using Nourishify.API.Shared.Persistence.Context;
using Nourishify.API.Shared.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add API Documentation Information
    
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "Nourishify API",
        Description = "Nourishify RESTful API",
        TermsOfService = new Uri("https://nourishify.com/tos"),
        Contact = new OpenApiContact
        {
            Name = "Nourishify.studio",
            Url = new Uri("https://nourishify.studio")
        },
        License = new OpenApiLicense
        {
            Name = "Nourishify Resources License",
            Url = new Uri("https://nourishify.com/license")
        }
    });
    options.EnableAnnotations();
    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            Array.Empty<string>()
        }
    });
});

// Add Database Connection

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySQL(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors());

// Add lowercase routes
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Shared Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// AppSettings Configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Dependency Injection Configuration
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Security Injection Configuration
builder.Services.AddScoped<IJwtHandler, JwtHandler>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// AutoMapper Configuration
builder.Services.AddAutoMapper(
    typeof(ModelToResourceProfile),
    typeof(ResourceToModelProfile),
    
    typeof(ModelToResourceProfile),
    typeof(ResourceToModelProfile));


var app = builder.Build();

// Validation for ensuring Database Objects are created
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<AppDbContext>())
{
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
    });
}
// Configure CORS 
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure Error Handler Middleware
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure JWT Handling
app.UseMiddleware<JwtMiddleware>();

//Default
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();