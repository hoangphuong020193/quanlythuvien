using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetBookByBookCode
{
    public interface IGetBookByBookCodeQuery
    {
        Task<BookViewModel> ExecuteAsync(string bookCode);
    }
}
