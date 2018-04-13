using Library.Library.Book.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetListNewBook
{
    public interface IGetListBookNewQuery
    {
        Task<List<BookViewModel>> ExecuteAsync();
    }
}
