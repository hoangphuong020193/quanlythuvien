using Library.Common.Paging;
using Library.Library.Admin.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Admin.Queries.GetListUserNotReturnBook
{
    public interface IGetListUserNotReturnBookQuery
    {
        Task<PagedList<UserNotReturnBookViewModel>> ExecuteAsync(int page, int pageSize);
    }
}
