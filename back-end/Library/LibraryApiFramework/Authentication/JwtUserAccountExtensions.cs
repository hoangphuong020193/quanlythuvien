using Library.ApiFramework.AppSetting.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library.ApiFramework.Authentication
{
    public static class JwtUserAccountExtensions
    {
        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public static string GenerateToken(this JwtUserAccount account, JwtOptions jwtOptions)
        {
            try
            {
                jwtOptions.IssuedAt = DateTime.UtcNow;
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, account.UserName),
                    new Claim(ClaimTypes.NameIdentifier, account.UserId.ToString()),
                    new Claim("Authenticated", "True"),
                    new Claim("userId", account.UserId.ToString()??"0"),
                    new Claim("displayName", account.DisplayName??string.Empty),
                    new Claim("fistName", account.FirstName??string.Empty),
                    new Claim("lastName", account.LastName??string.Empty),
                    new Claim("middleName", account.MiddleName??string.Empty),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64)
                };

                // Create the JWT and write it to a string
                JwtSecurityToken jwtToken = new JwtSecurityToken(
                    issuer: jwtOptions.Issuer,
                    audience: jwtOptions.Audience,
                    claims: claims,
                    notBefore: jwtOptions.NotBefore,
                    expires: jwtOptions.Expiration,
                    signingCredentials: jwtOptions.SigningCredentials);

                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                return token;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
