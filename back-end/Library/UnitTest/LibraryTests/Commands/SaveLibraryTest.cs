using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Library.Commands.SaveLibrary;
using Library.Library.Library.Queries.GetListLibrary;
using Library.Library.Library.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.LibraryTests.Commands
{
    public class SaveLibraryTest
    {
        private IDbContext _dbContext;

        public SaveLibraryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("SaveLibraryTest" + Guid.NewGuid()).Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task SaveLibraryNull_Test()
        {
            LibraryViewModel library1 = null;

            LibraryViewModel library2 = new LibraryViewModel()
            {
                Id = 0,
                Name = "",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };

            var efRepository = new EfRepository<Libraries>(_dbContext);
            var saveLibraryCommand = new SaveLibraryCommand(efRepository);

            var result1 = await saveLibraryCommand.ExecuteAsync(library1);
            var result2 = await saveLibraryCommand.ExecuteAsync(library2);

            Assert.Equal((int)HttpStatusCode.NotAcceptable, result1.GetFirstErrorCode());
            Assert.Equal((int)HttpStatusCode.NotAcceptable, result2.GetFirstErrorCode());
        }

        [Fact]
        public async Task SaveLibraryUpdateFail_Test()
        {
            LibraryViewModel library = new LibraryViewModel()
            {
                Id = 1,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };
            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Libraries>(_dbContext);
            var saveLibraryCommand = new SaveLibraryCommand(efRepository);

            var result = await saveLibraryCommand.ExecuteAsync(library);

            Assert.Equal((int)HttpStatusCode.NotFound, result.GetFirstErrorCode());
        }

        [Fact]
        public async Task SaveLibraryUpdateSuccess_Test()
        {
            Libraries library1 = new Libraries()
            {
                Id = 1,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };
            _dbContext.Set<Libraries>().Add(library1);

            LibraryViewModel model = new LibraryViewModel()
            {
                Id = 1,
                Name = "abc123",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Libraries>(_dbContext);
            var saveLibraryCommand = new SaveLibraryCommand(efRepository);
            var result = await saveLibraryCommand.ExecuteAsync(model);

            var getListLibrary = new GetListLibraryQuery(efRepository);
            var library = (await getListLibrary.ExecuteAsync()).FirstOrDefault();

            Assert.Equal(result.Data, model.Id);
            Assert.Equal(model.Name, library.Name);
        }

        [Fact]
        public async Task SaveLibraryInsertSuccess_Test()
        {
            LibraryViewModel model = new LibraryViewModel()
            {
                Id = 0,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com",
                Enabled = true
            };

            var efRepository = new EfRepository<Libraries>(_dbContext);
            var saveLibraryCommand = new SaveLibraryCommand(efRepository);
            var result = await saveLibraryCommand.ExecuteAsync(model);

            var getListLibrary = new GetListLibraryQuery(efRepository);
            var library = (await getListLibrary.ExecuteAsync()).FirstOrDefault();

            Assert.Equal(result.Data, model.Id);
            Assert.Equal(model.Name, library.Name);
            Assert.Equal(model.Address, library.Address);
            Assert.Equal(model.Phone, library.Phone);
            Assert.Equal(model.Email, library.Email);
            Assert.Equal(model.Enabled, library.Enabled);
        }
    }
}

