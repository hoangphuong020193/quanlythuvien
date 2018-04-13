using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Cart.Queries.GetBookInCartForBorrow;
using Library.Library.Cart.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.CartTest.Queries
{
    public class GetBookInCartForBorrowTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public GetBookInCartForBorrowTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetBookInCartForBorrowTest" + Guid.NewGuid()).Options;
            _dbContext = new ApplicationDbContext(options);

            var container = new DryIoc.Container()
                          .WithDependencyInjectionAdapter(new ServiceCollection())
                          .ConfigureServiceProvider<CompositionRoot>();

            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = container,
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", "1") }))
                }
            };

            _httpContextAccessor = httpContextAccessor;
        }

        [Fact]
        public async Task GetBookInCartForBorrow_Test()
        {

            var bookCart = new BookCarts
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Status = (int)BookStatus.InOrder,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCart);

            var book = new Books
            {
                Id = 1,
                BookName = "Book",
                BookCode = "ABC",
                LibraryId = 1,
                CategoryId = 1,
                SupplierId = 1,
                PublisherId = 1,
                Author = "Author",
                AmountAvailable = 10,
                Amount = 20,
                Enabled = true,
                MaximumDateBorrow = 30
            };
            _dbContext.Set<Books>().Add(book);

            var library = new Libraries
            {
                Id = 1,
                Name = "library",
                Enabled = true
            };
            _dbContext.Set<Libraries>().Add(library);

            await _dbContext.SaveChangesAsync();

            var bookCartRepository = new EfRepository<BookCarts>(_dbContext);
            var bookRepository = new EfRepository<Books>(_dbContext);
            var libraryRepository = new EfRepository<Libraries>(_dbContext);

            var getBookInCartForBorrowQuery = new GetBookInCartForBorrowQuery(bookCartRepository, bookRepository, libraryRepository, _httpContextAccessor);
            var bookInCartBorrow = await getBookInCartForBorrowQuery.ExecuteAsync();
            Assert.NotNull(bookInCartBorrow);
            Assert.Single(bookInCartBorrow);
            BookInCartDetailViewModel bookInCart = bookInCartBorrow.FirstOrDefault();
            Assert.Equal(book.Id, bookInCart.BookId);
            Assert.Equal(book.BookCode, bookInCart.BookCode);
            Assert.Equal(book.BookCode, bookInCart.BookCode);
            Assert.Equal(book.BookName, bookInCart.BookName);
            Assert.Equal(book.Author, bookInCart.Author);
            Assert.Equal(book.Amount, bookInCart.Amoun);
            Assert.Equal(book.AmountAvailable, bookInCart.AmountAvailable);
            Assert.Equal(book.MaximumDateBorrow, bookInCart.MaximumDateBorrow);
            Assert.Equal(bookCart.ModifiedDate, bookInCart.ModifiedDate);
            Assert.Equal(bookCart.Status, bookInCart.Status);
            Assert.Equal(library.Name, bookInCart.LibraryName);
            Assert.Equal(DateTime.Now.Date.AddDays(book.MaximumDateBorrow + 1), bookInCart.ReturnDate);
        }
    }
}
