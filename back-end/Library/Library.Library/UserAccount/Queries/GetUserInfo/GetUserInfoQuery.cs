using System.Linq;
using System.Threading.Tasks;
using Library.Common;
using Library.Data.Entities.Account;
using Library.Data.Services;
using Library.Library.UserAccount.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.UserAccount.Queries.GetUserInfo
{
    public class GetUserInfoQuery : IGetUserInfoQuery
    {
        private readonly IRepository<Users> _userRepository;
        private readonly HttpContext _httpContext;

        public GetUserInfoQuery(IRepository<Users> userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<UserViewModel> ExecuteAsync()
        {
            var userId = int.Parse(_httpContext?.User?.UserId());
            var user = await _userRepository
                .TableNoTracking
                .Where(x => x.Id == userId && x.Enabled.Value)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName
                }).FirstOrDefaultAsync();
            return user;
        }
    }
}
