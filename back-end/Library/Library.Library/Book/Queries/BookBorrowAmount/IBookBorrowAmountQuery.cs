using Library.Common.Paging;
using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.BookBorrowAmount
{
    public interface IBookBorrowAmountQuery
    {
        Task<PagedList<BookBorrowAmountViewModel>> ExecuteAsync(int page, int pageSize, string startDate, string endDate);
    }
}
