using Library.Library.Category.Commands.SaveCategory;
using Library.Library.Category.Queries.GetCategory;
using Library.Library.Category.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.API.Controllers.User
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly IGetCategoryQuery _getCategoryQuery;
        private readonly ISaveCategoryCommand _saveCategoryCommand;

        public CategoryController(
            IGetCategoryQuery getCategoryQuery,
            ISaveCategoryCommand saveCategoryCommand)
        {
            _getCategoryQuery = getCategoryQuery;
            _saveCategoryCommand = saveCategoryCommand;
        }

        [HttpGet]
        [Route("ReturnCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> ReturnCategoryAsync()
        {
            var result = await _getCategoryQuery.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpPost]
        [Route("SaveCategory")]
        public async Task<IActionResult> SaveCategoryAsync([FromBody] CategoryViewModel model)
        {
            var result = await _saveCategoryCommand.ExecuteAsync(model);
            return new ObjectResult(result.Data);
        }
    }
}
