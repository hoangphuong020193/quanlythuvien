using Library.Library.Cart.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Common;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Library.Common.Enum;
using System;

namespace Library.Library.Cart.Queries.GetBookInCartForBorrow
{
    public class GetBookInCartForBorrowQuery : IGetBookInCartForBorrowQuery
    {
        private readonly IRepository<BookCarts> _bookCartRepository;
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<Libraries> _libraryRepository;
        private readonly HttpContext _httpContext;

        public GetBookInCartForBorrowQuery(
            IRepository<BookCarts> bookCartRepository,
            IRepository<Books> bookRepository,
            IRepository<Libraries> libraryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookCartRepository = bookCartRepository;
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<List<BookInCartDetailViewModel>> ExecuteAsync()
        {
            var userId = int.Parse(_httpContext?.User?.UserId());
            var result = await (from cart in _bookCartRepository.TableNoTracking.Where(x => x.UserId == userId && x.Status == (int)BookStatus.InOrder)
                                join book in _bookRepository.TableNoTracking on cart.BookId equals book.Id
                                join lib in _libraryRepository.TableNoTracking on book.LibraryId equals lib.Id
                                select new BookInCartDetailViewModel
                                {
                                    BookId = book.Id,
                                    BookCode = book.BookCode,
                                    BookName = book.BookName,
                                    Author = book.Author,
                                    Amoun = book.Amount.Value,
                                    AmountAvailable = book.AmountAvailable.Value,
                                    MaximumDateBorrow = book.MaximumDateBorrow,
                                    ModifiedDate = cart.ModifiedDate.Value,
                                    Status = cart.Status,
                                    ReturnDate = DateTime.Now.Date.AddDays(book.MaximumDateBorrow + 1),
                                    LibraryName = lib.Name
                                }).ToListAsync();

            return result;
        }
    }
}
