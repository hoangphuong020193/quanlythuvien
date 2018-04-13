using HRM.CrossCutting.Command;
using System.Threading.Tasks;

namespace Library.Library.Book.Commands.TakenBook
{
    public interface ITakenBookCommand
    {
        Task<CommandResult> ExecuteAsync(int userId, string bookCode, int requestId);
    }
}
