using DryIoc.Microsoft.DependencyInjection;
using Library.ApiFramework.IoCRegistrar;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Favorite.Commands.UpdateBookFavorite;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.FavoriteTest.Commands
{
    public class UpdateBookFavoriteTest
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public UpdateBookFavoriteTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("UpdateBookFavoriteTest" + Guid.NewGuid()).EnableSensitiveDataLogging().Options;
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
        public async Task InsertBookFavorite_Test()
        {
            Books book = new Books()
            {
                Id = 1
            };
            _dbContext.Set<Books>().Add(book);
            _dbContext.SaveChanges();

            var efRepository = new EfRepository<BookFavorites>(_dbContext);

            _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", "1")
            }));

            var updateBookFavoriteCommand = new UpdateBookFavoriteCommand(efRepository, _httpContextAccessor);
            var result = await updateBookFavoriteCommand.ExecuteAsync(1);
            Assert.True(result);
        }
    }
}
