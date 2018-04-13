using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Supplier.Commands.SaveSupplier;
using Library.Library.Supplier.Queries.GetListSupplier;
using Library.Library.Supplier.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.SupplierTest.Commands
{
    public class SaveSupplierTest
    {
        private IDbContext _dbContext;

        public SaveSupplierTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("SaveSupplierTest" + Guid.NewGuid()).Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task SaveSupplierNull_Test()
        {
            SupplierViewModel supplier1 = null;

            SupplierViewModel supplier2 = new SupplierViewModel()
            {
                Id = 0,
                Name = "",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };

            var efRepository = new EfRepository<Suppliers>(_dbContext);
            var saveSupplierCommand = new SaveSupplierCommand(efRepository);

            var result1 = await saveSupplierCommand.ExecuteAsync(supplier1);
            var result2 = await saveSupplierCommand.ExecuteAsync(supplier2);

            Assert.Equal((int)HttpStatusCode.NotAcceptable, result1.GetFirstErrorCode());
            Assert.Equal((int)HttpStatusCode.NotAcceptable, result2.GetFirstErrorCode());
        }

        [Fact]
        public async Task SaveSupplierUpdateFail_Test()
        {
            SupplierViewModel supplier = new SupplierViewModel()
            {
                Id = 1,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };

            var efRepository = new EfRepository<Suppliers>(_dbContext);
            var saveSupplierCommand = new SaveSupplierCommand(efRepository);

            var result = await saveSupplierCommand.ExecuteAsync(supplier);

            Assert.Equal((int)HttpStatusCode.NotFound, result.GetFirstErrorCode());
        }

        [Fact]
        public async Task SaveSupplierUpdateSuccess_Test()
        {
            Suppliers supplier1 = new Suppliers()
            {
                Id = 1,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };
            _dbContext.Set<Suppliers>().Add(supplier1);

            SupplierViewModel model = new SupplierViewModel()
            {
                Id = 1,
                Name = "abc123",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Suppliers>(_dbContext);
            var saveSupplierCommand = new SaveSupplierCommand(efRepository);
            var result = await saveSupplierCommand.ExecuteAsync(model);

            var getListSupplier = new GetListSupplier(efRepository);
            var supplier = (await getListSupplier.ExecuteAsync()).FirstOrDefault();

            Assert.Equal(result.Data, model.Id);
            Assert.Equal(model.Name, supplier.Name);
        }

        [Fact]
        public async Task SaveSupplierInsertSuccess_Test()
        {
            SupplierViewModel model = new SupplierViewModel()
            {
                Id = 0,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com",
                Enabled = true
            };

            var efRepository = new EfRepository<Suppliers>(_dbContext);
            var saveSupplierCommand = new SaveSupplierCommand(efRepository);
            var result = await saveSupplierCommand.ExecuteAsync(model);

            var getListSupplier = new GetListSupplier(efRepository);
            var supplier = (await getListSupplier.ExecuteAsync()).FirstOrDefault();

            Assert.Equal(result.Data, model.Id);
            Assert.Equal(model.Name, supplier.Name);
            Assert.Equal(model.Address, supplier.Address);
            Assert.Equal(model.Phone, supplier.Phone);
            Assert.Equal(model.Email, supplier.Email);
            Assert.Equal(model.Enabled, supplier.Enabled);
        }
    }
}
