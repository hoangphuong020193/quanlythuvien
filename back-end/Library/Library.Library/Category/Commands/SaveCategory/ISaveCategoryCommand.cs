using HRM.CrossCutting.Command;
using Library.Library.Category.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Category.Commands.SaveCategory
{
    public interface ISaveCategoryCommand
    {
        Task<CommandResult> ExecuteAsync(CategoryViewModel model);
    }
}
