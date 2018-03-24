using Library.Library.Supplier.Commands.SaveSupplier;
using Library.Library.Supplier.Queries.GetListSupplier;
using Library.Library.Supplier.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.API.Controllers.Publisher
{
    [Route("api/[controller]")]
    public class SupplierController : Controller
    {
        private readonly IGetListSupplier _getListSupplier;
        private readonly ISaveSupplierCommand _saveSupplierCommand;


        public SupplierController(
             IGetListSupplier getListSupplier,
             ISaveSupplierCommand saveSupplierCommand)
        {
            _getListSupplier = getListSupplier;
            _saveSupplierCommand = saveSupplierCommand;
        }

        [HttpGet]
        [Route("ReturnListSupplier")]
        public async Task<IActionResult> ReturnListSupplierAsync()
        {
            var result = await _getListSupplier.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpPost]
        [Route("SaveSupplier")]
        public async Task<IActionResult> SaveSupplierAsync([FromBody] SupplierViewModel model)
        {
            var result = await _saveSupplierCommand.ExecuteAsync(model);
            return new ObjectResult(result.Data);
        }
    }
}
