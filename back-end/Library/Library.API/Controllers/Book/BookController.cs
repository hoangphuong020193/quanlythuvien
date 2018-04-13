using Library.Library.BookRequest.Queries.GetRequestInfoByCode;
using Library.Library.Book.Commands.CancelBook;
using Library.Library.Book.Commands.ReturnBook;
using Library.Library.Book.Commands.SaveBook;
using Library.Library.Book.Commands.SaveBookImage;
using Library.Library.Book.Commands.TakenBook;
using Library.Library.Book.Queries.BookBorrowAmount;
using Library.Library.Book.Queries.CheckBookExistsCode;
using Library.Library.Book.Queries.GetBookDetail;
using Library.Library.Book.Queries.GetBookPhoto;
using Library.Library.Book.Queries.GetBookSection;
using Library.Library.Book.Queries.GetBookViewByCategory;
using Library.Library.Book.Queries.GetListBook;
using Library.Library.Book.Queries.GetListBookByRequestCode;
using Library.Library.Book.Queries.GetListNewBook;
using Library.Library.Book.ViewModels;
using Library.Library.Favorite.Commands.UpdateBookFavorite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace Library.API.Controllers.Book
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IGetListBookNewQuery _getListBookNewQuery;
        private readonly IGetBookDetailQuery _getBookDetailQuery;
        private readonly IGetBookPhotoQuery _getBookPhotoQuery;
        private readonly IUpdateBookFavoriteCommand _updateBookFavoriteCommand;
        private readonly IGetBookSectionQuery _getBookSectionQuery;
        private readonly IGetBookViewByCategoryQuery _getBookViewByCategoryQuery;
        private readonly IGetListBookByRequestCodeQuery _getListBookByRequestCodeQuery;
        private readonly IGetRequestInfoByCodeQuery _getRequestInfoByCodeQuery;
        private readonly ITakenBookCommand _takenBookCommand;
        private readonly IReturnBookCommand _returnBookCommand;
        private readonly ICancelBookCommand _cancelBookCommand;
        private readonly IGetListBookQuery _getListBookQuery;
        private readonly ICheckBookCodeExistsQuery _checkBookCodeExistsQuery;
        private readonly ISaveBookCommand _saveBookCommand;
        private readonly ISaveBookImageCommand _saveBookImageCommand;
        private readonly IBookBorrowAmountQuery _bookBorrowAmountQuery;

        public BookController(IGetListBookNewQuery getListBookNewQuery,
            IGetBookDetailQuery getBookDetailQuery,
            IGetBookPhotoQuery getBookPhotoQuery,
            IUpdateBookFavoriteCommand updateBookFavoriteCommand,
            IGetBookSectionQuery getBookSectionQuery,
            IGetBookViewByCategoryQuery getBookViewByCategoryQuery,
            IGetListBookByRequestCodeQuery getListBookByRequestCodeQuery,
            IGetRequestInfoByCodeQuery getRequestInfoByCodeQuery,
            ITakenBookCommand takenBookCommand,
            IReturnBookCommand returnBookCommand,
            ICancelBookCommand cancelBookCommand,
            IGetListBookQuery getListBookQuery,
            ICheckBookCodeExistsQuery checkBookCodeExistsQuery,
            ISaveBookCommand saveBookCommand,
            ISaveBookImageCommand saveBookImageCommand,
            IBookBorrowAmountQuery bookBorrowAmountQuery)
        {
            _getListBookNewQuery = getListBookNewQuery;
            _getBookDetailQuery = getBookDetailQuery;
            _getBookPhotoQuery = getBookPhotoQuery;
            _updateBookFavoriteCommand = updateBookFavoriteCommand;
            _getBookSectionQuery = getBookSectionQuery;
            _getBookViewByCategoryQuery = getBookViewByCategoryQuery;
            _getListBookByRequestCodeQuery = getListBookByRequestCodeQuery;
            _getRequestInfoByCodeQuery = getRequestInfoByCodeQuery;
            _takenBookCommand = takenBookCommand;
            _returnBookCommand = returnBookCommand;
            _cancelBookCommand = cancelBookCommand;
            _getListBookQuery = getListBookQuery;
            _checkBookCodeExistsQuery = checkBookCodeExistsQuery;
            _saveBookCommand = saveBookCommand;
            _saveBookImageCommand = saveBookImageCommand;
            _bookBorrowAmountQuery = bookBorrowAmountQuery;
        }

        [HttpGet]
        [Route("ReturnListNewBook")]
        [AllowAnonymous]
        public async Task<IActionResult> ReturnListNewBookAsync()
        {
            var result = await _getListBookNewQuery.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnBookDetail/{bookCode=}")]
        [AllowAnonymous]
        public async Task<IActionResult> ReturnBookDetailAsync(string bookCode)
        {
            var result = await _getBookDetailQuery.ExecuteAsync(bookCode);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("Photo/{bookCode=}")]
        [AllowAnonymous]
        public async Task<IActionResult> PhotoAsync(string bookCode)
        {
            var result = await _getBookPhotoQuery.ExecuteAsync(bookCode);
            if (result == null)
            {
                return BadRequest();
            }

            return new FileContentResult(result.Content, "image/jpeg");
        }

        [HttpPost]
        [Route("UserFavoriteBook")]
        public async Task<IActionResult> UserFavoriteBookAsync([FromBody] int bookId)
        {
            var result = await _updateBookFavoriteCommand.ExecuteAsync(bookId);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnTopBookInSection/{take:int=10}")]
        [AllowAnonymous]
        public async Task<IActionResult> ReturnTopBookInSectionAsync(int take, string sectionType)
        {
            var result = await _getBookSectionQuery.ExecuteAsync(sectionType, take);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("BookView/{page:int=0}/{pageSize:int=1}")]
        [AllowAnonymous]
        public async Task<IActionResult> BookViewAsync(int page, int pageSize, string view)
        {
            var result = await _getBookViewByCategoryQuery.ExecuteAsync(view, page, pageSize);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnListBookByCode/{libraryId:int=-3}")]
        public async Task<IActionResult> ReturnListBookByCodeAsync(int libraryId, string code)
        {
            var result = await _getListBookByRequestCodeQuery.ExecuteAsync(code, libraryId);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnRequestInfo")]
        public async Task<IActionResult> ReturnRequestInfoAsync(string code)
        {
            var result = await _getRequestInfoByCodeQuery.ExecuteAsync(code);
            return new ObjectResult(result);
        }

        [HttpPut]
        [Route("TakenBook")]
        public async Task<IActionResult> TakenBookAsync([FromBody] JObject jsonObject)
        {
            int userId = int.Parse(jsonObject["userId"].ToString());
            string bookCode = jsonObject["bookCode"].ToString();
            int requestId = int.Parse(jsonObject["requestId"].ToString());

            var result = await _takenBookCommand.ExecuteAsync(userId, bookCode, requestId);
            return new ObjectResult(result.Succeeded);
        }

        [HttpPut]
        [Route("ReturnBook")]
        public async Task<IActionResult> ReturnBookAsync([FromBody] JObject jsonObject)
        {
            int userId = int.Parse(jsonObject["userId"].ToString());
            string bookCode = jsonObject["bookCode"].ToString();
            int requestId = int.Parse(jsonObject["requestId"].ToString());

            var result = await _returnBookCommand.ExecuteAsync(userId, bookCode, requestId);
            return new ObjectResult(result.Succeeded);
        }

        [HttpPut]
        [Route("CancelBook")]
        public async Task<IActionResult> CancelBookAsync([FromBody] JObject jsonObject)
        {
            int userId = int.Parse(jsonObject["userId"].ToString());
            string bookCode = jsonObject["bookCode"].ToString();
            int requestId = int.Parse(jsonObject["requestId"].ToString());

            var result = await _cancelBookCommand.ExecuteAsync(userId, bookCode, requestId);
            return new ObjectResult(result.Succeeded);
        }

        [HttpGet]
        [Route("ReturnListBook/{page:int=0}/{pageSize:int=1}")]
        public async Task<IActionResult> ReturnListBookAsync(int page, int pageSize, string search)
        {
            var result = await _getListBookQuery.ExecuteAsync(page, pageSize, search);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("CheckBookCodeExists/{bookId:int=0}/{bookCode=}")]
        public async Task<IActionResult> CheckBookCodeExistsAsync(int bookId, string bookCode)
        {
            var result = await _checkBookCodeExistsQuery.ExecuteAsync(bookId, bookCode);
            return new ObjectResult(result);
        }

        [HttpPost]
        [Route("SaveBook")]
        public async Task<IActionResult> SaveBookAsync([FromBody] BookViewModel model)
        {
            var result = await _saveBookCommand.ExecuteAsync(model);
            return new ObjectResult(result.Succeeded ? result.Data
                :
                new
                {
                    code = result.Errors.First().Code,
                    description = result.Errors.First().Description
                });
        }

        [HttpPost]
        [Route("SaveImage/{bookId:int=0}")]
        public async Task<IActionResult> SaveImageAsync(IFormFile file, int bookId)
        {
            using (Stream stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);
                    var result = await _saveBookImageCommand.ExecuteAsync(fileContent, bookId);
                    return new ObjectResult(result.Succeeded);
                }
            }
        }

        [HttpGet]
        [Route("TopBook/{page:int=0}/{pageSize=}/{startDate=}/{endDate=}")]
        public async Task<IActionResult> TopBookAsync(int page, int pageSize, string startDate, string endDate)
        {
            var result = await _bookBorrowAmountQuery.ExecuteAsync(page, pageSize, startDate, endDate);
            return new ObjectResult(result);
        }
    }
}
