using Library.Common.Paging;
using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetBookBorrow
{
    public interface IGetBookBorrowQuery
    {
        Task<PagedList<BookBorrowViewModel>> ExecuteAsync(int page, int pageSize, string status);
    }
}
