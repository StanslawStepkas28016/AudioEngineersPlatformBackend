namespace AudioEngineersPlatformBackend.Application;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // services.AddScoped<IRecruitmentsService, RecruitmentsService>();
        return services;
    }
}