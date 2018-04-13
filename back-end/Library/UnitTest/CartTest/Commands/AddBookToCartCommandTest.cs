using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Cart.Commands.AddBookToCart;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.CartTest.Commands
{
    public class AddBookToCartCommandTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public AddBookToCartCommandTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("AddBookToCartCommandTest" + Guid.NewGuid()).Options;
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
        public async Task AddBookToCartCommandTest_Test()
        {
            var bookCarts1 = new BookCarts
            {
                Id = 10,
                UserId = 1,
                BookId = 1,
                Status = (int)BookStatus.InOrder,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCarts1);

            var bookCarts2 = new BookCarts
            {
                Id = 20,
                UserId = 1,
                BookId = 2,
                Status = (int)BookStatus.Waiting,
                ModifiedDate = DateTime.Now
            };
            _dbContext.Set<BookCarts>().Add(bookCarts2);

            await _dbContext.SaveChangesAsync();

            var bookCartRepository = new EfRepository<BookCarts>(_dbContext);

            var addBookToCartCommand = new AddBookToCartCommand(bookCartRepository, _httpContextAccessor);
            List<int> bookIds = new List<int>() { 3, 4 };
            var result = await addBookToCartCommand.ExecuteAsync(bookIds);
            Assert.True(result.Succeeded);
        }      
    }
}
