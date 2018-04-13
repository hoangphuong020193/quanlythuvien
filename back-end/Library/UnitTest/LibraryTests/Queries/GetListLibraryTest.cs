using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Library.Queries.GetListLibrary;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.LibraryTests.Queries
{
    public class GetListLibraryTest
    {
        private IDbContext _dbContext;

        public GetListLibraryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetListLibraryTest").Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetListLibrary_Test()
        {
            var library1 = new Libraries
            {
                Id = 1,
                Name = "library1",
                Enabled = true
            };
            _dbContext.Set<Libraries>().Add(library1);

            var library2 = new Libraries
            {
                Id = 2,
                Name = "library1",
                Enabled = false
            };
            _dbContext.Set<Libraries>().Add(library2);

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Libraries>(_dbContext);
            var getListLibrary = new GetListLibraryQuery(efRepository);
            var libraries = await getListLibrary.ExecuteAsync();
            Assert.NotNull(libraries);
            Assert.Equal(2, libraries.Count);
        }
    }
}
