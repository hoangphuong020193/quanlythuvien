
using Library.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Library.ApiFramework.Authorization
{
    public static class AuthorizationServiceCollectionExtension
    {
        public static IServiceCollection ConfigurePermissions(this IServiceCollection services)
        {
            services.AddAuthorization(opts => opts.AddPolicy(PermissionGroup.AdminLibrary, policy => policy.RequireClaim("Permission", PermissionGroup.AdminLibrary)));
            return services;
        }
    }
}
