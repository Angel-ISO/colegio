
using System.Text;
using Aplicacion.UnitOfWork;
using AspNetCoreRateLimit;
using ColegioAPI.Helpers;
using ColegioAPI.Services;
using Dominio;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;

namespace ColegioAPI.Extensions;

public static class ApplicationServiceExtension
{
  public static void ConfigureCors(this IServiceCollection services) =>
services.AddCors(options =>
{
options.AddPolicy("CorsPolicy", builder =>
             builder.AllowAnyOrigin()    //WithOrigins("https://domini.com")
             .AllowAnyMethod()           //WithMethods(*GET", "POST", "PUT", "DELETE")
             .AllowAnyHeader());         //WithHeaders(*accept*, "content-type")
});


  public static void AddAplicationService(this IServiceCollection services)
  {
    // services.AddScoped<IMoviesInterface, MovieRepository>();
    // services.AddScoped<IGenreInterface, GenreRepository>();
    // services.AddScoped<IDirectorInterface, DirectorRepository>();
    // services.AddScoped<IJwtGenerator,JwtGenerator>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddAutoMapper(typeof(ApplicationServiceExtension));
    services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IAuthorizationHandler, GlobalVerbRoleHandler>(); 
  }



  public static void ConfigureRateLimiting(this IServiceCollection services)
  {
    services.AddMemoryCache();
    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    services.AddInMemoryRateLimiting();
    services.Configure<IpRateLimitOptions>(options =>
    {

      options.EnableEndpointRateLimiting = true;
      options.StackBlockedRequests = false;
      options.HttpStatusCode = 429;
      options.RealIpHeader = "localhost";
      options.GeneralRules = new List<RateLimitRule>
         {
           new RateLimitRule
           {
             Endpoint = "*",
             Period  ="10s",
             Limit = 2
           }
         };
    });
  }

  public static void ConfigureApiVersioning(this IServiceCollection services)
  {
    services.AddApiVersioning(Options =>
    {


      Options.DefaultApiVersion = new ApiVersion(1, 0);
      Options.AssumeDefaultVersionWhenUnspecified = true;
      Options.ApiVersionReader = ApiVersionReader.Combine(
          new QueryStringApiVersionReader("ver"),
          new HeaderApiVersionReader("X-Version")
      );
      Options.ReportApiVersions = true;

    });
  }


  public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
  {
    services.Configure<JWT>(configuration.GetSection("JWT"));

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(o =>
        {
          o.RequireHttpsMetadata = false;
          o.SaveToken = false;
          o.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
          };
        });
  }


}
