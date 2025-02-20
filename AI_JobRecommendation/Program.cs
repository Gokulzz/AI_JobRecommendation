using System.Security.Claims;
using System.Text;
using app.BLL;
using app.BLL.DTO;
using app.BLL.Services;
using app.DAL.Data;
using ConfigureManager.cs;
using FluentAssertions.Common;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;

try
{

    var builder = WebApplication.CreateBuilder(args);
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadName()
        .Enrich.WithThreadId()
        .Enrich.WithProcessId()
        .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
    logger.Information("App starting");
    //we will bind the properties of emailconfiguration properties from our appsettings.json to type emailconfiguration which is our DTO
    //for email Configuration
    //var emailConfig = builder.Configuration
    //    .GetSection("EmailConfiguration")
    //    .Get<EmailConfigurationDTO>();
    //builder.Services.AddSingleton(emailConfig);
    builder.Services.Configure<EmailConfigurationDTO>(builder.Configuration.GetSection("EmailConfiguration"));
    builder.Services.ConfigureDependency();
    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("V1", new OpenApiInfo
        {
            Version = "V1",
            Title = "WebAPI",
            Description = "AIJobRecommendationApp WebAPI"
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Bearer Authentication with JWT Token",
            Type = SecuritySchemeType.Http
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List <string>()
        }
        });
    });
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
                ValidIssuer = builder.Configuration.GetSection("JWT").GetSection("ValidIssuer").Value,
                ValidAudience = builder.Configuration.GetSection("JWT").GetSection("ValidAudience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT")
                .GetSection("SecretKey").Value)),
                NameClaimType = ClaimTypes.NameIdentifier
            };
            //to set userIdentifier for SignalR
            // Enable JWT authentication for SignalR
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for SignalR hub
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/userActivityHub"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };

        });
   
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {

            var connectionString = builder.Configuration.GetSection("RedisServerURL").Value;
  
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Redis connection string is not configured.");
                }
                var configuration = ConfigurationOptions.Parse(connectionString, true);
                return ConnectionMultiplexer.Connect(configuration);
            
          

        });
    builder.Services.AddSignalR();


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin", policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
    });



    var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/V1/swagger.json", "AIJobRecommendationApp WebAPI");
        });
    }
    app.UseExceptionMiddleware();

    app.UseHttpsRedirection();

    app.UseCors("AllowSpecificOrigin");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapHub<UserActivity>("/userActivityHub");
    app.UseStaticFiles();
    app.MapControllers();
    app.Run();
}
catch(Exception ex)
{
    Console.Write(ex.ToString());   
}
finally
{
    Log.CloseAndFlush();
}