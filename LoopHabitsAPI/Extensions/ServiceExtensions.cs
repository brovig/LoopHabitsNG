﻿using Contracts;
using LoggerService;

namespace LoopHabitsAPI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}
