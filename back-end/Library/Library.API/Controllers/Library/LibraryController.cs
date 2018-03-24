using Library.Library.Library.Commands.SaveLibrary;
using Library.Library.Library.Queries.GetListLibrary;
using Library.Library.Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.API.Controllers.Publisher
{
    [Route("api/[controller]")]
    public class LibraryController : Controller
    {
        private readonly IGetListLibraryQuery _getListLibraryQuery;
        private readonly ISaveLibraryCommand _saveLibraryCommand;

        public LibraryController(
            IGetListLibraryQuery getListLibraryQuery,
            ISaveLibraryCommand saveLibraryCommand)
        {
            _getListLibraryQuery = getListLibraryQuery;
            _saveLibraryCommand = saveLibraryCommand;
        }

        [HttpGet]
        [Route("ReturnListLibrary")]
        public async Task<IActionResult> ReturnListLibraryAsync()
        {
            var result = await _getListLibraryQuery.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpPost]
        [Route("SaveLibrary")]
        public async Task<IActionResult> SaveLibraryAsync([FromBody] LibraryViewModel model)
        {
            var result = await _saveLibraryCommand.ExecuteAsync(model);
            return new ObjectResult(result.Data);
        }
    }
}
