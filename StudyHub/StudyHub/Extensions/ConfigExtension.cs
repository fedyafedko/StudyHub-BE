using StudyHub.Common.Configs;

namespace StudyHub.Extensions;

public static class ConfigExtension
{
    public static IServiceCollection AddConfig<T>(this IServiceCollection services, IConfiguration configuration)
        where T : ConfigBase
    {
        return services.Configure<T>(configuration.GetSection(typeof(T).Name));
    }
}
