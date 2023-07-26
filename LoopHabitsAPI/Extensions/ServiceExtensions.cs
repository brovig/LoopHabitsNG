using System.Runtime.CompilerServices;

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
}
