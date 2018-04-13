using Library.Common.Paging;
using Library.Library.Admin.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Admin.Queries.GetReadStatistic
{
    public interface IReadStatisticQuery
    {
        Task<PagedList<ReadStatisticViewModel>> ExecuteAsync(int page, int pageSize, string startDate, string endDate, int groupBy);
    }
}
