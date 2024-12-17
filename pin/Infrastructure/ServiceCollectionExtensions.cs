

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace pin.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<T>(
            this IServiceCollection services) where T : class, new()
        {
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var configuration = (IConfigurationRoot)provider.GetService<IConfiguration>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new WritableOptions<T>(options, configuration);
            });
        }
    }
}
