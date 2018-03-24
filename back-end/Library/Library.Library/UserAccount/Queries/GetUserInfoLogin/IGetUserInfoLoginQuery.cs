using Library.Library.UserAccount.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.UserAccount.Queries.GetUserInfoLogin
{
    public interface IGetUserInfoLoginQuery
    {
        Task<UserLoginViewModel> ExecuteAsync(string userName);
    }
}
