using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Cart.Queries.GetListBookInCart;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.CartTest.Queries
{
    public class GetListBookInCartTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public GetListBookInCartTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetListBookInCartTest" + Guid.NewGuid()).Options;
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
        public async Task GetListBookInCart_Test()
        {
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
                Status = (int)BookStatus.Pending,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCarts2);

            var bookCarts3 = new BookCarts
            {
                Id = 3,
                UserId = 1,
                BookId = 3,
                Status = (int)BookStatus.Waiting,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCarts3);

            await _dbContext.SaveChangesAsync();

            var bookCartRepository = new EfRepository<BookCarts>(_dbContext);
            var bookRepository = new EfRepository<Books>(_dbContext);
            var libraryRepository = new EfRepository<Libraries>(_dbContext);

            var getListBookInCartQuery = new GetListBookInCartQuery(bookCartRepository, _httpContextAccessor);
            var bookInCart = await getListBookInCartQuery.ExecuteAsync();
            Assert.NotNull(bookInCart);
            Assert.Equal(2, bookInCart.Count);
        }
    }
}
