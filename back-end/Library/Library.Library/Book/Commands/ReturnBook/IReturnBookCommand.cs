using HRM.CrossCutting.Command;
using System.Threading.Tasks;

namespace Library.Library.Book.Commands.ReturnBook
{
    public interface IReturnBookCommand
    {
        Task<CommandResult> ExecuteAsync(int userId, string bookCode, int requestId);
    }
}
