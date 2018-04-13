using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetBookViewByCategory
{
    public interface IGetBookViewByCategoryQuery
    {
        Task<SearchBookResultViewModel> ExecuteAsync(string view, int page, int pageSize);
    }
}
