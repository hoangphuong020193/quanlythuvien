using HRM.CrossCutting.Command;
using Library.Library.Supplier.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Supplier.Commands.SaveSupplier
{
    public interface ISaveSupplierCommand
    {
        Task<CommandResult> ExecuteAsync(SupplierViewModel model);
    }
}
