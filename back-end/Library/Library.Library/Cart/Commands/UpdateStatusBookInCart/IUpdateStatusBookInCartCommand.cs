using HRM.CrossCutting.Command;
using System.Threading.Tasks;

namespace Library.Library.Cart.Commands.UpdateStatusBookInCart
{
    public interface IUpdateStatusBookInCartCommand
    {
        Task<CommandResult> ExecuteAsync(int bookId, int status);
    }
}
