using Library.Library.Book.Queries.GetBookBorrow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.API.Controllers.MyBook
{
    [Route("api/[controller]")]
    public class MyBookController : Controller
    {
        private readonly IGetBookBorrowQuery _getBookBorrowQuery;

        public MyBookController(IGetBookBorrowQuery getBookBorrowQuery)
        {
            _getBookBorrowQuery = getBookBorrowQuery;
        }

        [HttpGet]
        [Route("ReturnMyBookList/{page:int=0}/{pageSize:int=1}")]
        [AllowAnonymous]
        public async Task<IActionResult> ReturnMyBookListAsync(int page, int pageSize, string status)
        {
            var result = await _getBookBorrowQuery.ExecuteAsync(page, pageSize, status);
            return new ObjectResult(result);
        }
    }
}
