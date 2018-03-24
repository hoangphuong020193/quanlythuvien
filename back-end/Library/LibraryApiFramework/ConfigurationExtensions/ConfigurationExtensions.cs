using Microsoft.Extensions.Configuration;
using System;

namespace Library.ApiFramework.ConfigurationExtensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDbConnectionString(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            return configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}
