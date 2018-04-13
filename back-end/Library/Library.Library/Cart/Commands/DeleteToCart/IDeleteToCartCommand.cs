using HRM.CrossCutting.Command;
using System.Threading.Tasks;

namespace Library.Library.Cart.Commands.DeleteToCart
{
    public interface IDeleteToCartCommand
    {
        Task<CommandResult> ExecuteAsync(int bookId);
    }
}
