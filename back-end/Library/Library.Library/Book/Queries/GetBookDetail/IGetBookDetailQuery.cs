using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetBookDetail
{
    public interface IGetBookDetailQuery
    {
        Task<BookViewModel> ExecuteAsync(string bookCode);
    }
}
