using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Supplier.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Supplier.Queries.GetListSupplier
{
    public class GetListSupplier : IGetListSupplier
    {
        private readonly IRepository<Suppliers> _supplierRepository;

        public GetListSupplier(IRepository<Suppliers> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<List<SupplierViewModel>> ExecuteAsync()
        {
            var result = await _supplierRepository.TableNoTracking.Select(x => new SupplierViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email,
                Enabled = x.Enabled.Value
            }).ToListAsync();

            return result;
        }
    }
}
