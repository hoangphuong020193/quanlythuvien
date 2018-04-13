using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.User.Queries.GetUserNotification;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.UserTest.Queries
{
    public class GetUserNotificationTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public GetUserNotificationTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetUserNotificationTest" + Guid.NewGuid()).Options;
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
        public async Task GetUserNotification_Test()
        {
            var Notification1 = new UserNotifications
            {
                Id = 1,
                UserId = 1,
                Message = "message1",
                MessageDate = DateTime.Now
            };
            _dbContext.Set<UserNotifications>().Add(Notification1);

            var Notification2 = new UserNotifications
            {
                Id = 2,
                UserId = 1,
                Message = "message2",
                MessageDate = DateTime.Now
            };
            _dbContext.Set<UserNotifications>().Add(Notification2);

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<UserNotifications>(_dbContext);
            var getUserNotificationQuery = new GetUserNotificationQuery(efRepository, _httpContextAccessor);
            var notifications = await getUserNotificationQuery.ExecuteAsync();
            Assert.NotNull(notifications);
            Assert.Equal(2, notifications.Count);
        }

        [Fact]
        public async Task GetUserNotificationVerifyData_Test()
        {
            var notification = new UserNotifications
            {
                Id = 1,
                UserId = 1,
                Message = "message1",
                MessageDate = DateTime.Now.AddDays(-3)
            };
            _dbContext.Set<UserNotifications>().Add(notification);

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<UserNotifications>(_dbContext);
            var getUserNotificationQuery = new GetUserNotificationQuery(efRepository, _httpContextAccessor);
            var result = (await getUserNotificationQuery.ExecuteAsync()).FirstOrDefault();
            Assert.NotNull(result);
            Assert.Equal(notification.Message, result.Message);
            Assert.Equal(notification.MessageDate, result.MessageDate);
            Assert.Equal((DateTime.Now.Date - notification.MessageDate).Days < 2, result.IsNew);
        }
    }
}
