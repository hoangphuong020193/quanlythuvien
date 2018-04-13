using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Data.Entities.Account;
using Library.Data.Services;
using Library.Library.UserAccount.Queries.GetUserInfo;
using Library.Library.UserAccount.Queries.GetUserInfoLogin;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.UserAccountTests.Queries
{
    public class GetUserInfoTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;
        private EfRepository<Users> efRepository;

        public GetUserInfoTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetUserInfoTest" + Guid.NewGuid().ToString()).Options;
            _dbContext = new ApplicationDbContext(options);

            var container = new DryIoc.Container()
               .WithDependencyInjectionAdapter(new ServiceCollection())
               .ConfigureServiceProvider<CompositionRoot>();

            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = container,
                    // User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", "1") }))
                }
            };

            _httpContextAccessor = httpContextAccessor;

            InintDataTest();
        }

        private void InintDataTest()
        {
            var User1 = new Users
            {
                Id = 1,
                UserName = "User1",
                Password = "1234567",
                Enabled = true
            };
            _dbContext.Set<Users>().Add(User1);

            var User2 = new Users
            {
                Id = 2,
                UserName = "User2",
                Password = "7654321",
                Enabled = true
            };
            _dbContext.Set<Users>().Add(User2);

            _dbContext.SaveChanges();
            efRepository = new EfRepository<Users>(_dbContext);
        }

        [Fact]
        public async Task GetUserInfoLogin_Test()
        {
            var getUserInfoQuery = new GetUserInfoLoginQuery(efRepository);
            var user = await getUserInfoQuery.ExecuteAsync("User1");
            Assert.NotNull(user);
            Assert.Equal("User1", user.UserName);
        }

        [Fact]
        public async Task GetUserInfoLoginNull_Test()
        {
            var getUserInfoQuery = new GetUserInfoLoginQuery(efRepository);
            var user = await getUserInfoQuery.ExecuteAsync("User");
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserInfo_Test()
        {
            _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", "1")
            }));

            await _dbContext.SaveChangesAsync();

            var efRepository = new EfRepository<Users>(_dbContext);
            var getUserInfoQuery = new GetUserInfoQuery(efRepository, _httpContextAccessor);
            var user = await getUserInfoQuery.ExecuteAsync();
            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal("User1", user.UserName);
        }
    }
}
