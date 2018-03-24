using Library.Library.User.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.User.Queries.GetUserNotification
{
    public interface IGetUserNotificationQuery
    {
        Task<List<UserNotificationViewModel>> ExecuteAsync();
    }
}
