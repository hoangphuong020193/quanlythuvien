using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.SearchBook
{
    public interface ISearchBookQuery
    {
        Task<SearchBookResultViewModel> ExecuteAsync(string search, int page, int pageSize);
    }
}
