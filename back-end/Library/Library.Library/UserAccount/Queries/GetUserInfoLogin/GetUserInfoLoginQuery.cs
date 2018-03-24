using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Account;
using Library.Data.Services;
using Library.Library.UserAccount.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.UserAccount.Queries.GetUserInfoLogin
{
    public class GetUserInfoLoginQuery : IGetUserInfoLoginQuery
    {
        private readonly IRepository<Data.Entities.Account.Users> _userRepository;

        public GetUserInfoLoginQuery(IRepository<Data.Entities.Account.Users> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserLoginViewModel> ExecuteAsync(string userName)
        {
            var user = await _userRepository
                .TableNoTracking
                .Where(x => x.UserName == userName && x.Enabled.Value)
                .Select(x => new UserLoginViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    PassWord = x.Password,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName
                }).FirstOrDefaultAsync();
            return user;
        }
    }
}
