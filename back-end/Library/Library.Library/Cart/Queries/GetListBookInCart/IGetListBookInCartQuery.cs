using Library.Library.Cart.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Cart.Queries.GetListBookInCart
{
    public interface IGetListBookInCartQuery
    {
        Task<List<BookInCartViewModel>> ExecuteAsync();
    }
}
