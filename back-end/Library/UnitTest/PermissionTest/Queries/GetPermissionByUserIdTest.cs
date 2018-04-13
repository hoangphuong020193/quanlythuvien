using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Permission.Queries.GetPermissionByUserId;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.PermissionTest.Queries
{
    public class GetPermissionByUserIdTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public GetPermissionByUserIdTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetPermissionByUserIdTest" + Guid.NewGuid()).Options;
            _dbContext = new ApplicationDbContext(options);

            var container = new DryIoc.Container()
               .WithDependencyInjectionAdapter(new ServiceCollection())
               .ConfigureServiceProvider<CompositionRoot>();

            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = container,
                }
            };

            _httpContextAccessor = httpContextAccessor;
        }

        [Fact]
        public async Task GetPermissionUserLogin_Test()
        {
            PermissionGroups group1 = new PermissionGroups()
            {
                Id = 1,
                GroupName = "Group1",
                Enabled = true
            };
            _dbContext.Set<PermissionGroups>().Add(group1);

            PermissionGroups group2 = new PermissionGroups()
            {
                Id = 2,
                GroupName = "Group2",
                Enabled = true
            };
            _dbContext.Set<PermissionGroups>().Add(group2);

            PermissionGroupMembers member1 = new PermissionGroupMembers()
            {
                Id = 1,
                UserId = 1,
                GroupId = 1,
                Enabled = true
            };
            _dbContext.Set<PermissionGroupMembers>().Add(member1);

            PermissionGroupMembers member2 = new PermissionGroupMembers()
            {
                Id = 2,
                UserId = 1,
                GroupId = 2,
                Enabled = true
            };
            _dbContext.Set<PermissionGroupMembers>().Add(member2);

            _dbContext.SaveChanges();
            var efRepositoryPermissionGroups = new EfRepository<PermissionGroups>(_dbContext);
            var efRepositoryPermissionGroupMembers = new EfRepository<PermissionGroupMembers>(_dbContext);

            _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", "1")
            }));

            var getPermissionByUserIdQuery = new GetPermissionByUserIdQuery(efRepositoryPermissionGroups, efRepositoryPermissionGroupMembers, _httpContextAccessor);
            var permission = await getPermissionByUserIdQuery.ExecuteAsync();
            Assert.NotNull(permission);
            Assert.Equal(2, permission.Count);
        }

        [Fact]
        public async Task GetPermissionUser_Test()
        {
            PermissionGroups group1 = new PermissionGroups()
            {
                Id = 1,
                GroupName = "Group1",
                Enabled = true
            };
            _dbContext.Set<PermissionGroups>().Add(group1);

            PermissionGroups group2 = new PermissionGroups()
            {
                Id = 2,
                GroupName = "Group2",
                Enabled = true
            };
            _dbContext.Set<PermissionGroups>().Add(group2);

            PermissionGroupMembers member1 = new PermissionGroupMembers()
            {
                Id = 1,
                UserId = 1,
                GroupId = 1,
                Enabled = true
            };
            _dbContext.Set<PermissionGroupMembers>().Add(member1);

            PermissionGroupMembers member2 = new PermissionGroupMembers()
            {
                Id = 2,
                UserId = 1,
                GroupId = 2,
                Enabled = true
            };
            _dbContext.Set<PermissionGroupMembers>().Add(member2);

            _dbContext.SaveChanges();
            var efRepositoryPermissionGroups = new EfRepository<PermissionGroups>(_dbContext);
            var efRepositoryPermissionGroupMembers = new EfRepository<PermissionGroupMembers>(_dbContext);

            _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", "2")
            }));

            var getPermissionByUserIdQuery = new GetPermissionByUserIdQuery(efRepositoryPermissionGroups, efRepositoryPermissionGroupMembers, _httpContextAccessor);
            var permission = await getPermissionByUserIdQuery.ExecuteAsync(1);
            Assert.NotNull(permission);
            Assert.Equal(2, permission.Count);
        }
    }
}
