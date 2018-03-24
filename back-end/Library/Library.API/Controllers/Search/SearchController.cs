using Library.Library.Book.Queries.SearchBook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.API.Controllers.Search
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearchBookQuery _searchBookQuery;

        public SearchController(ISearchBookQuery searchBookQuery)
        {
            _searchBookQuery = searchBookQuery;
        }

        [HttpGet]
        [Route("SearchBook/{page:int=0}/{pageSize:int=1}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchBookAsync(int page, int pageSize, string search)
        {
            var result = await _searchBookQuery.ExecuteAsync(search, page, pageSize);
            return new ObjectResult(result);
        }
    }
}
