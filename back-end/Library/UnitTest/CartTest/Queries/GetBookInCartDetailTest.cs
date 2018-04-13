using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Cart.Queries.GetBookInCartDetail;
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
    public class GetBookInCartDetailTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public GetBookInCartDetailTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetBookInCartDetailTest" + Guid.NewGuid()).Options;
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
        public async Task GetBookInCartDetailNoData_Test()
        {

            var library = new Libraries
            {
                Id = 1,
                Name = "library",
                Enabled = true
            };
            _dbContext.Set<Libraries>().Add(library);

            var book1 = new Books
            {
                Id = 1,
                BookName = "Book1",
                LibraryId = 1,
                CategoryId = 1,
                SupplierId = 1,
                PublisherId = 1,
                AmountAvailable = 10,
                Amount = 20,
                Enabled = true
            };
            _dbContext.Set<Books>().Add(book1);

            var book2 = new Books
            {
                Id = 2,
                BookName = "Book2",
                LibraryId = 1,
                CategoryId = 1,
                SupplierId = 1,
                PublisherId = 1,
                AmountAvailable = 10,
                Amount = 20,
                Enabled = true
            };
            _dbContext.Set<Books>().Add(book2);

            var bookCarts1 = new BookCarts
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Status = (int)BookStatus.InOrder,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCarts1);

            var bookCarts2 = new BookCarts
            {
                Id = 2,
                UserId = 1,
                BookId = 2,
                Status = (int)BookStatus.Waiting,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCarts2);

            await _dbContext.SaveChangesAsync();

            var bookCartRepository = new EfRepository<BookCarts>(_dbContext);
            var bookRepository = new EfRepository<Books>(_dbContext);
            var libraryRepository = new EfRepository<Libraries>(_dbContext);

            var getBookInCartDetailQuery = new GetBookInCartDetailQuery(bookCartRepository, bookRepository, libraryRepository, _httpContextAccessor);
            var bookInCart = await getBookInCartDetailQuery.ExecuteAsync();
            Assert.NotNull(bookInCart);
            Assert.Equal(2, bookInCart.Count);
        }

        [Fact]
        public async Task GetBookInCartDetailHaveData_Test()
        {

            var library = new Libraries
            {
                Id = 1,
                Name = "library",
                Enabled = true
            };
            _dbContext.Set<Libraries>().Add(library);

            var book1 = new Books
            {
                Id = 1,
                BookName = "Book1",
                LibraryId = 1,
                CategoryId = 1,
                SupplierId = 1,
                PublisherId = 1,
                AmountAvailable = 10,
                Amount = 20,
                Enabled = true
            };
            _dbContext.Set<Books>().Add(book1);

            var book2 = new Books
            {
                Id = 2,
                BookName = "Book2",
                LibraryId = 1,
                CategoryId = 1,
                SupplierId = 1,
                PublisherId = 1,
                AmountAvailable = 10,
                Amount = 20,
                Enabled = true
            };
            _dbContext.Set<Books>().Add(book2);

            var bookCarts1 = new BookCarts
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Status = (int)BookStatus.Returned,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCarts1);

            var bookCarts2 = new BookCarts
            {
                Id = 2,
                UserId = 1,
                BookId = 2,
                Status = (int)BookStatus.Cancel,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCarts2);

            await _dbContext.SaveChangesAsync();

            var bookCartRepository = new EfRepository<BookCarts>(_dbContext);
            var bookRepository = new EfRepository<Books>(_dbContext);
            var libraryRepository = new EfRepository<Libraries>(_dbContext);

            var getBookInCartDetailQuery = new GetBookInCartDetailQuery(bookCartRepository, bookRepository, libraryRepository, _httpContextAccessor);
            var bookInCart = await getBookInCartDetailQuery.ExecuteAsync();
            Assert.NotNull(bookInCart);
            Assert.False(bookInCart.Any());
        }
    }
}
