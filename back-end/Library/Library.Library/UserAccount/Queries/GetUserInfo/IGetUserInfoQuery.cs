using Library.Library.UserAccount.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.UserAccount.Queries.GetUserInfo
{
    public interface IGetUserInfoQuery
    {
        Task<UserViewModel> ExecuteAsync();
    }
}
