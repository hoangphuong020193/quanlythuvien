using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Category.Queries.GetCategory;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.CategoryTest.Queries
{
    public class GetCategoryTest
    {
        private IDbContext _dbContext;

        public GetCategoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetCategoryTest").Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetListLibrary_Test()
        {
            var category1 = new Categories
            {
                Id = 1,
                CategoryName = "category1",
                Type = "Book",
                Enabled = true
            };
            _dbContext.Set<Categories>().Add(category1);

            var category2 = new Categories
            {
                Id = 2,
                CategoryName = "category1",
                Type = "Book",
                Enabled = false
            };
            _dbContext.Set<Categories>().Add(category2);

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Categories>(_dbContext);
            var getListLibrary = new GetCategoryQuery(efRepository);
            var categories = await getListLibrary.ExecuteAsync();
            Assert.NotNull(categories);
            Assert.Single(categories);
        }
    }
}
