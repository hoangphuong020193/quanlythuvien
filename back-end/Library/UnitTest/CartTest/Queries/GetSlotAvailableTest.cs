using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Cart.Queries.GetSlotAvailable;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.CartTest.Queries
{
    public class GetSlotAvailableTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public GetSlotAvailableTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetSlotAvailableTest" + Guid.NewGuid()).Options;
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
        public async Task GetSlotAvailable_Test()
        {
            UserBooks userBook1 = new UserBooks()
            {
                Id = 1,
                UserId = 1,
                RequestId = 1,
                BookId = 1,
                ReceiveDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(3),
                DeadlineDate = DateTime.Now.AddHours(24),
                Status = (int)BookStatus.Borrowing
            };
            _dbContext.Set<UserBooks>().Add(userBook1);

            UserBooks userBook2 = new UserBooks()
            {
                Id = 2,
                UserId = 1,
                RequestId = 1,
                BookId = 1,
                ReceiveDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(3),
                DeadlineDate = DateTime.Now.AddHours(24),
                Status = (int)BookStatus.Pending
            };
            _dbContext.Set<UserBooks>().Add(userBook2);

            UserBooks userBook3 = new UserBooks()
            {
                Id = 3,
                UserId = 1,
                RequestId = 1,
                BookId = 1,
                ReceiveDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(3),
                DeadlineDate = DateTime.Now.AddHours(24),
                Status = (int)BookStatus.Waiting
            };
            _dbContext.Set<UserBooks>().Add(userBook3);

            await _dbContext.SaveChangesAsync();
            var userBooksRepository = new EfRepository<UserBooks>(_dbContext);
            var getSlotAvailableQuery = new GetSlotAvailableQuery(userBooksRepository, _httpContextAccessor);
            var slotAvaiable = await getSlotAvailableQuery.ExecuteAsync();
            Assert.Equal(8, slotAvaiable);
        }
    }
}
