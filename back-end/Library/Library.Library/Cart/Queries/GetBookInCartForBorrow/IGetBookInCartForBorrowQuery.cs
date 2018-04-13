using Library.Library.Cart.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Cart.Queries.GetBookInCartForBorrow
{
    public interface IGetBookInCartForBorrowQuery
    {
        Task<List<BookInCartDetailViewModel>> ExecuteAsync();
    }
}
