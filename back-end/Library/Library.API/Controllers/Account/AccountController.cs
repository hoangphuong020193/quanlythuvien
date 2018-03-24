using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Library.ApiFramework.Authentication;
using Microsoft.Extensions.Options;
using Library.ApiFramework.AppSetting.Options;
using Library.Common;
using Library.Library.UserAccount.ViewModels;
using Library.Library.UserAccount.Queries.GetUserInfoLogin;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IGetUserInfoLoginQuery _getUserInfoLogin;
        private readonly IOptionsSnapshot<JwtOptions> _jwtConfiguration;

        public AccountController(IGetUserInfoLoginQuery getUserInfoLogin,
            IOptionsSnapshot<JwtOptions> jwtConfiguration)
        {
            _getUserInfoLogin = getUserInfoLogin;
            _jwtConfiguration = jwtConfiguration;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel loginModel)
        {
            var employee = await _getUserInfoLogin.ExecuteAsync(loginModel.Username);
            if (employee == null)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            if (string.Compare(loginModel.Password.ToMD5(), employee.PassWord, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            var jwtOptions = _jwtConfiguration.Value;
            jwtOptions.ValidFor = TimeSpan.FromHours(24);
            var jwtUserAccount = new JwtUserAccount
            {
                UserId = employee.Id,
                UserName = employee.UserName,
                DisplayName = (employee.FirstName + " " + employee.MiddleName + " " + employee.LastName).Replace("  ", " "),
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName
            };

            return new ObjectResult(new { accessToken = jwtUserAccount.GenerateToken(jwtOptions), expired = jwtOptions.Expiration });
        }

        [HttpGet("PingAPI")]
        [AllowAnonymous]
        public string PingAPI()
        {
            return "Good";
        }
    }
}
