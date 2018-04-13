using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Supplier.Queries.GetListSupplier;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.SupplierTest.Queries
{
    public class GetListSupplierTest
    {
        private IDbContext _dbContext;

        public GetListSupplierTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("GetListSupplierTest").Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetListSupplier_Test()
        {
            var supplier1 = new Suppliers
            {
                Id = 1,
                Name = "Supplier1",
                Enabled = true
            };
            _dbContext.Set<Suppliers>().Add(supplier1);

            var supplier2 = new Suppliers
            {
                Id = 2,
                Name = "Supplier2",
                Enabled = false
            };
            _dbContext.Set<Suppliers>().Add(supplier2);

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Suppliers>(_dbContext);
            var getListSupplier = new GetListSupplier(efRepository);
            var suppliers = await getListSupplier.ExecuteAsync();
            Assert.NotNull(suppliers);
            Assert.Equal(2, suppliers.Count);
        }
    }
}
