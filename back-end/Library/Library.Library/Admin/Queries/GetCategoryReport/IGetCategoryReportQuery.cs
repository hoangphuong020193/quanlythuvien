using Library.Common.Paging;
using Library.Library.Admin.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Admin.Queries.GetCategoryReport
{
    public interface IGetCategoryReportQuery
    {
        Task<PagedList<CategoryReportViewModel>> ExecuteAsync(int page, int pageSize, string searchString);
    }
}
