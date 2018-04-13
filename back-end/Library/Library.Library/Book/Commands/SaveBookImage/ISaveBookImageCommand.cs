using HRM.CrossCutting.Command;
using System.Threading.Tasks;

namespace Library.Library.Book.Commands.SaveBookImage
{
    public interface ISaveBookImageCommand
    {
        Task<CommandResult> ExecuteAsync(byte[] img, int bookId);
    }
}
