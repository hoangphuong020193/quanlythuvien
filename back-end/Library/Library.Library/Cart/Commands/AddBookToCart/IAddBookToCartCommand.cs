using HRM.CrossCutting.Command;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Cart.Commands.AddBookToCart
{
    public interface IAddBookToCartCommand
    {
        Task<CommandResult> ExecuteAsync(List<int> bookIds);
    }
}
