using Library.Library.Supplier.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Supplier.Queries.GetListSupplier
{
    public interface IGetListSupplier
    {
        Task<List<SupplierViewModel>> ExecuteAsync();
    }
}
