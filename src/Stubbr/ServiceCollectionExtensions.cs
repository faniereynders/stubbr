using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Stubbr;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStubs(this IServiceCollection services, IConfigurationSection config)
        {
            return services.Configure<Dictionary<string, StubResponse>>(config);
        }
    }

}