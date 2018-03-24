using System.Security.Claims;

namespace Library.Common
{
    public static class ClaimsPrincipalExtensions
    {
        public static string UserId(this ClaimsPrincipal principal)
            => principal.FindFirst("userId")?.Value;

        public static string UserName(this ClaimsPrincipal principal) => principal.FindFirst(ClaimTypes.Name)?.Value;
    }
}
