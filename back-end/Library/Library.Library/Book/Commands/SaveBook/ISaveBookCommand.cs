using HRM.CrossCutting.Command;
using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Commands.SaveBook
{
    public interface ISaveBookCommand
    {
        Task<CommandResult> ExecuteAsync(BookViewModel model);
    }
}
