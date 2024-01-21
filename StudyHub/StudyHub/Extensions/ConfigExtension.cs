using StudyHub.Common.Configs;

namespace StudyHub.Extensions;

public static class ConfigExtension
{
    public static IServiceCollection AddConfig<T>(this IServiceCollection services)
            where T : ConfigBase, new()
    {
        IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        T configInstance = new T();
        services.Configure<T>(configuration.GetSection(typeof(T).Name));
        configuration.GetSection(typeof(T).Name).Bind(configInstance);
        services.AddSingleton<T>(configInstance);
        return services;
    }
}
