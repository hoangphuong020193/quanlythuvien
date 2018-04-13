using HRM.CrossCutting.Command;
using System.Threading.Tasks;

namespace Library.Library.Cart.Commands.BorrowBook
{
    public interface IBorrowBookCommand
    {
        Task<CommandResult> ExecuteAsync();
    }
}
