using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Library.ApiFramework.AppConfig
{
    public static class AppConfigExtensions
    {
        public static string[] GetAllowOrigins(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            return configuration["Auth:AllowOrigins"].Split(',').Select(val => val.Trim()).ToArray();
        }
    }
}
