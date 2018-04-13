using Library.Common.Paging;
using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetListBook
{
    public interface IGetListBookQuery
    {
        Task<PagedList<BookViewModel>> ExecuteAsync(int page, int pageSize, string search);
    }
}
