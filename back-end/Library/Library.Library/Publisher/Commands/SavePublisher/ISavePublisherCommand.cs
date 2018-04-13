using HRM.CrossCutting.Command;
using Library.Library.Publisher.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Publisher.Commands.SavePublisher
{
    public interface ISavePublisherCommand
    {
        Task<CommandResult> ExecuteAsync(PublisherViewModel model);
    }
}
