using HRM.CrossCutting.Command;
using Library.Library.Library.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Library.Commands.SaveLibrary
{
    public interface ISaveLibraryCommand
    {
        Task<CommandResult> ExecuteAsync(LibraryViewModel model);
    }
}
