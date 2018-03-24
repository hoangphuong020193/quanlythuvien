using Library.Library.Cart.Commands.AddBookToCart;
using Library.Library.Cart.Commands.BorrowBook;
using Library.Library.Cart.Commands.DeleteToCart;
using Library.Library.Cart.Commands.UpdateStatusBookInCart;
using Library.Library.Cart.Queries.GetBookInCartDetail;
using Library.Library.Cart.Queries.GetBookInCartForBorrow;
using Library.Library.Cart.Queries.GetListBookInCart;
using Library.Library.Cart.Queries.GetSlotAvailable;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.API.Controllers.Cart
{
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly IGetBookInCartForBorrowQuery _getBookInCartForBorrowQuery;
        private readonly IGetListBookInCartQuery _getListBookInCartQuery;
        private readonly IGetBookInCartDetailQuery _getBookInCartDetailQuery;
        private readonly IAddBookToCartCommand _addBookToCartCommand;
        private readonly IDeleteToCartCommand _deleteToCartCommand;
        private readonly IUpdateStatusBookInCartCommand _updateStatusBookInCartCommand;
        private readonly IGetSlotAvailableQuery _getSlotAvailableQuery;
        private readonly IBorrowBookCommand _borrowBookCommand;

        public CartController(
            IGetBookInCartForBorrowQuery getBookInCartForBorrowQuery,
            IGetListBookInCartQuery getListBookInCartQuery,
            IGetBookInCartDetailQuery getBookInCartDetailQuery,
            IAddBookToCartCommand addBookToCartCommand,
            IDeleteToCartCommand deleteToCartCommand,
            IUpdateStatusBookInCartCommand updateStatusBookInCartCommand,
            IGetSlotAvailableQuery getSlotAvailableQuery,
            IBorrowBookCommand borrowBookCommand)
        {
            _getBookInCartForBorrowQuery = getBookInCartForBorrowQuery;
            _getListBookInCartQuery = getListBookInCartQuery;
            _getBookInCartDetailQuery = getBookInCartDetailQuery;
            _addBookToCartCommand = addBookToCartCommand;
            _deleteToCartCommand = deleteToCartCommand;
            _updateStatusBookInCartCommand = updateStatusBookInCartCommand;
            _getSlotAvailableQuery = getSlotAvailableQuery;
            _borrowBookCommand = borrowBookCommand;
        }

        [HttpGet]
        [Route("ReturnBookInCartForBorrow")]
        public async Task<IActionResult> ReturnBookInCartForBorrowAsync()
        {
            var result = await _getBookInCartForBorrowQuery.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnBookInCart")]
        public async Task<IActionResult> ReturnBookInCartAsync()
        {
            var result = await _getListBookInCartQuery.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnBookInCartDetail")]
        public async Task<IActionResult> ReturnBookInCartDetailAsync()
        {
            var result = await _getBookInCartDetailQuery.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpPost]
        [Route("AddBookInCart")]
        public async Task<IActionResult> AddBookInCartAsync([FromBody] List<int> bookIds)
        {
            var result = await _addBookToCartCommand.ExecuteAsync(bookIds);
            return new ObjectResult(result.Data);
        }

        [HttpDelete]
        [Route("DeleteBookInCart/{bookId:int}")]
        public async Task<IActionResult> DelteBookInCartAsync(int bookId)
        {
            var result = await _deleteToCartCommand.ExecuteAsync(bookId);
            return new ObjectResult(result);
        }

        [HttpPut]
        [Route("UpdateStatusBookInCart/{bookId:int}")]
        public async Task<IActionResult> UpdateStatusBookInCartAsync(int bookId, [FromBody] int status)
        {
            var result = await _updateStatusBookInCartCommand.ExecuteAsync(bookId, status);
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("ReturnSlotAvailable")]
        public async Task<IActionResult> ReturnSlotAvailableAsync()
        {
            var result = await _getSlotAvailableQuery.ExecuteAsync();
            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("BorrowBook")]
        public async Task<IActionResult> BorrowBookAsync()
        {
            var result = await _borrowBookCommand.ExecuteAsync();
            return new ObjectResult(result);
        }
    }
}
