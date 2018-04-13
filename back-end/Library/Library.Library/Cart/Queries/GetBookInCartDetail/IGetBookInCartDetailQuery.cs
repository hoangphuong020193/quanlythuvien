using Library.Library.Cart.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Cart.Queries.GetBookInCartDetail
{
    public interface IGetBookInCartDetailQuery
    {
        Task<List<BookInCartDetailViewModel>> ExecuteAsync();
    }
}
