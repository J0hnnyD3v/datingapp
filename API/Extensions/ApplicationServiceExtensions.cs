﻿using Microsoft.EntityFrameworkCore;

using API.Data;
using API.Interfaces;
using API.Services;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<DataContext>(options =>
    {
      options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
    });

    services.AddCors();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUserRepository, UserRepository>();
    return services;
  }
}
