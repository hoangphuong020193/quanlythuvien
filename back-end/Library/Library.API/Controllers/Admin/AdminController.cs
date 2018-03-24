using Library.Library.Admin.Queries.GetBorrowStatus;
using Library.Library.Admin.Queries.GetCategoryReport;
using Library.Library.Admin.Queries.GetListUserNotReturnBook;
using Library.Library.Admin.Queries.GetReadStatistic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.API.Controllers.User
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IGetListUserNotReturnBookQuery _getListUserNotReturnBookQuery;
        private readonly IReadStatisticQuery _readStatisticQuery;
        private readonly IGetBorrowStatusQuery _getBorrowStatusQuery;
        private readonly IGetCategoryReportQuery _getCategoryReportQuery;

        public AdminController(
            IGetListUserNotReturnBookQuery getListUserNotReturnBookQuery,
            IReadStatisticQuery readStatisticQuery,
            IGetBorrowStatusQuery getBorrowStatusQuery,
            IGetCategoryReportQuery getCategoryReportQuery)
        {
            _getListUserNotReturnBookQuery = getListUserNotReturnBookQuery;
            _readStatisticQuery = readStatisticQuery;
            _getBorrowStatusQuery = getBorrowStatusQuery;
            _getCategoryReportQuery = getCategoryReportQuery;
        }

        [HttpGet]
        [Route("ReturnListUserNotReturnBook/{page:int=0}/{pageSize:int=0}")]
        public async Task<IActionResult> ReturnListUserNotReturnBookAsync(int page, int pageSize)
        {
            var result = await _getListUserNotReturnBookQuery.ExecuteAsync(page, pageSize);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnListReadStatistic/{page:int=0}/{pageSize:int=0}")]
        public async Task<IActionResult> ReturnListReadStatisticAsync(int page, int pageSize, string startDate, string endDate, int groupBy)
        {
            var result = await _readStatisticQuery.ExecuteAsync(page, pageSize, startDate, endDate, groupBy);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnBorrowStatus/{page:int=0}/{pageSize:int=0}")]
        public async Task<IActionResult> ReturnBorrowStatusAsync(int page, int pageSize, string startDate, string endDate, int libraryId, int status, string searchString)
        {
            var result = await _getBorrowStatusQuery.ExecuteAsync(page, pageSize, startDate, endDate, libraryId, status, searchString);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnCategoryReport/{page:int=0}/{pageSize:int=0}")]
        public async Task<IActionResult> ReturnCategoryReportAsync(int page, int pageSize, string searchString)
        {
            var result = await _getCategoryReportQuery.ExecuteAsync(page, pageSize, searchString);
            return new ObjectResult(result);
        }
    }
}
