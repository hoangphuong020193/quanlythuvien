using Library.Library.Book.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetListBookByRequestCode
{
    public interface IGetListBookByRequestCodeQuery
    {
        Task<List<BookBorrowViewModel>> ExecuteAsync(string code, int libraryId = -3);
    }
}
