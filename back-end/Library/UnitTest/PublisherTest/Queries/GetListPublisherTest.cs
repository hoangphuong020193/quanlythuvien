using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Publisher.Queries.GetListPublisher;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.PublisherTest.Queries
{
    public class GetListPublisherTest
    {
        private IDbContext _dbContext;

        public GetListPublisherTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetListPublisherTest").Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetListPublisher_Test()
        {
            var publisher1 = new Publishers
            {
                Id = 1,
                Name = "publishers1",
                Enabled = true
            };
            _dbContext.Set<Publishers>().Add(publisher1);

            var publisher2 = new Publishers
            {
                Id = 2,
                Name = "publisher2",
                Enabled = false
            };
            _dbContext.Set<Publishers>().Add(publisher2);

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Publishers>(_dbContext);
            var getListPublisherQuery = new GetListPublisherQuery(efRepository);
            var publishers = await getListPublisherQuery.ExecuteAsync();
            Assert.NotNull(publishers);
            Assert.Equal(2, publishers.Count);
        }
    }
}
