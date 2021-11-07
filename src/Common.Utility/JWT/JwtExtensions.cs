using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Common.Utility.JWT
{
    /// <summary>
    /// jwt配置文件扩展
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtOptions(this IServiceCollection services, Action<JwtOptions> jwtOptions)
        {
            if (jwtOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtOptions));
            }
            services.Configure(jwtOptions);
            services.AddSingleton<JwtHelper>();

            return services;
        }
    }
}
