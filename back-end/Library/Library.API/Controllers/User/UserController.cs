using Library.Library.Permission.Queries.GetPermissionByUserId;
using Library.Library.User.Queries.GetUserNotification;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.API.Controllers.User
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IGetUserNotificationQuery _getUserNotificationQuery;
        private readonly IGetPermissionByUserIdQuery _getPermissionByUserIdQuery;

        public UserController(
            IGetUserNotificationQuery getUserNotificationQuery,
            IGetPermissionByUserIdQuery getPermissionByUserIdQuery)
        {
            _getUserNotificationQuery = getUserNotificationQuery;
            _getPermissionByUserIdQuery = getPermissionByUserIdQuery;
        }

        [HttpGet]
        [Route("ReturnUserNotification")]
        public async Task<IActionResult> ReturnUserNotificationAsync()
        {
            var result = await _getUserNotificationQuery.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnUserPermission")]
        public async Task<IActionResult> ReturnUserPermissionAsync()
        {
            var result = await _getPermissionByUserIdQuery.ExecuteAsync();
            return new ObjectResult(result);
        }
    }
}
