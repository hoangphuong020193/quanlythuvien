using Library.Data.Entities.Account;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.BookRequest.Queries.GetRequestInfoByCode;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.BookRequestTest.Queries
{
    public class GetRequestInfoByCodeTest
    {
        private IDbContext _dbContext;

        public GetRequestInfoByCodeTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetRequestInfoByCodeTest" + Guid.NewGuid()).Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetRequestInfoByCodeNull_Test()
        {
            var efRepository = new EfRepository<UserBookRequests>(_dbContext);
            var getRequestInfoByCodeQuery = new GetRequestInfoByCodeQuery(efRepository);
            var result = await getRequestInfoByCodeQuery.ExecuteAsync(null);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRequestInfoByCodeHaveData_Test()
        {
            var user = new Users
            {
                Id = 1,
                UserName = "user1",
                FirstName = "A",
                MiddleName = "B",
                LastName = "C"
            };
            _dbContext.Set<Users>().Add(user);

            var userBookRequests = new UserBookRequests
            {
                Id = 1,
                UserId = 1,
                RequestCode = "ABCD",
                RequestDate = DateTime.Now,
                User = user
            };
            _dbContext.Set<UserBookRequests>().Add(userBookRequests);
            await _dbContext.SaveChangesAsync();

            var efRepository = new EfRepository<UserBookRequests>(_dbContext);
            var getRequestInfoByCodeQuery = new GetRequestInfoByCodeQuery(efRepository);
            var result = await getRequestInfoByCodeQuery.ExecuteAsync("ABCD");
            Assert.NotNull(result);
            Assert.Equal(userBookRequests.Id, result.RequestId);
            Assert.Equal(userBookRequests.RequestCode, result.RequestCode);
            Assert.Equal(userBookRequests.RequestDate, result.RequestDate);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.LastName + " " + user.MiddleName + " " + user.FirstName, result.FullName);
        }

        [Fact]
        public async Task GetRequestInfoByCodeNoData_Test()
        {
            var user = new Users
            {
                Id = 1,
                UserName = "user1",
                FirstName = "A",
                MiddleName = "B",
                LastName = "C"
            };
            _dbContext.Set<Users>().Add(user);

            var userBookRequests = new UserBookRequests
            {
                Id = 1,
                UserId = 1,
                RequestCode = "ABCD",
                RequestDate = DateTime.Now,
                User = user
            };
            _dbContext.Set<UserBookRequests>().Add(userBookRequests);
            await _dbContext.SaveChangesAsync();

            var efRepository = new EfRepository<UserBookRequests>(_dbContext);
            var getRequestInfoByCodeQuery = new GetRequestInfoByCodeQuery(efRepository);
            var result = await getRequestInfoByCodeQuery.ExecuteAsync("ABCDE");
            Assert.Null(result);
        }
    }
}
