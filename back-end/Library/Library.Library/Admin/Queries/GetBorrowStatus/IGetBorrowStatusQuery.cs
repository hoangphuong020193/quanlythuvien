using Library.Common.Paging;
using Library.Library.Admin.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Admin.Queries.GetBorrowStatus
{
    public interface IGetBorrowStatusQuery
    {
        Task<PagedList<BorrowStatusViewModel>> ExecuteAsync(int page, int pageSize, string startDate, string endDate, int libraryId, int status,string searchString);
    }
}
