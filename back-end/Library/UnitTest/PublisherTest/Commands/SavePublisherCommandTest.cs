using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Publisher.Commands.SavePublisher;
using Library.Library.Publisher.Queries.GetListPublisher;
using Library.Library.Publisher.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.PublisherTest.Commands
{
    public class SavePublisherCommandTest
    {
        private IDbContext _dbContext;

        public SavePublisherCommandTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("SavePublisherTest" + Guid.NewGuid()).Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task SavePublisherNull_Test()
        {
            PublisherViewModel publisher1 = null;

            PublisherViewModel publisher2 = new PublisherViewModel()
            {
                Id = 0,
                Name = "",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com",
                Enabled = true
            };

            var efRepository = new EfRepository<Publishers>(_dbContext);
            var savePublisherCommand = new SavePublisherCommand(efRepository);

            var result1 = await savePublisherCommand.ExecuteAsync(publisher1);
            var result2 = await savePublisherCommand.ExecuteAsync(publisher2);

            Assert.Equal((int)HttpStatusCode.NotAcceptable, result1.GetFirstErrorCode());
            Assert.Equal((int)HttpStatusCode.NotAcceptable, result2.GetFirstErrorCode());
        }

        [Fact]
        public async Task SavePublisherUpdateFail_Test()
        {
            PublisherViewModel publisher = new PublisherViewModel()
            {
                Id = 1,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };

            var efRepository = new EfRepository<Publishers>(_dbContext);
            var savePublisherCommand = new SavePublisherCommand(efRepository);

            var result = await savePublisherCommand.ExecuteAsync(publisher);

            Assert.Equal((int)HttpStatusCode.NotFound, result.GetFirstErrorCode());
        }

        [Fact]
        public async Task SavePublisherUpdateSuccess_Test()
        {
            Publishers publisher1 = new Publishers()
            {
                Id = 1,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };
            _dbContext.Set<Publishers>().Add(publisher1);

            PublisherViewModel model = new PublisherViewModel()
            {
                Id = 1,
                Name = "abc123",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com"
            };

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Publishers>(_dbContext);
            var savePublisherCommand = new SavePublisherCommand(efRepository);
            var result = await savePublisherCommand.ExecuteAsync(model);

            var getListPublisher = new GetListPublisherQuery(efRepository);
            var Publisher = (await getListPublisher.ExecuteAsync()).FirstOrDefault();

            Assert.Equal(result.Data, model.Id);
            Assert.Equal(model.Name, Publisher.Name);
        }

        [Fact]
        public async Task SavePublisherInsertSuccess_Test()
        {
            PublisherViewModel model = new PublisherViewModel()
            {
                Id = 0,
                Name = "abc",
                Address = "123",
                Phone = "123456789",
                Email = "abc@gmail.com",
                Enabled = true
            };

            var efRepository = new EfRepository<Publishers>(_dbContext);
            var savePublisherCommand = new SavePublisherCommand(efRepository);
            var result = await savePublisherCommand.ExecuteAsync(model);

            var getListPublisher = new GetListPublisherQuery(efRepository);
            var publisher = (await getListPublisher.ExecuteAsync()).FirstOrDefault();

            Assert.Equal(result.Data, model.Id);
            Assert.Equal(model.Name, publisher.Name);
            Assert.Equal(model.Address, publisher.Address);
            Assert.Equal(model.Phone, publisher.Phone);
            Assert.Equal(model.Email, publisher.Email);
            Assert.Equal(model.Enabled, publisher.Enabled);
        }
    }
}
