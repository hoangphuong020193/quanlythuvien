using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Category.Commands.SaveCategory;
using Library.Library.Category.Queries.GetCategory;
using Library.Library.Category.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.CategoryTest.Commands
{
    public class SaveCategoryTest
    {
        private IDbContext _dbContext;

        public SaveCategoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("SaveCategoryTest" + Guid.NewGuid()).Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task SaveCategoryNull_Test()
        {
            CategoryViewModel category1 = null;

            CategoryViewModel category2 = new CategoryViewModel()
            {
                Id = 0,
                CategoryName = "",
                Type = "123",
                Enabled = true
            };

            var efRepository = new EfRepository<Categories>(_dbContext);
            var saveCategoryCommand = new SaveCategoryCommand(efRepository);

            var result1 = await saveCategoryCommand.ExecuteAsync(category1);
            var result2 = await saveCategoryCommand.ExecuteAsync(category2);

            Assert.Equal((int)HttpStatusCode.NotAcceptable, result1.GetFirstErrorCode());
            Assert.Equal((int)HttpStatusCode.NotAcceptable, result2.GetFirstErrorCode());
        }

        [Fact]
        public async Task SaveCategoryUpdateFail_Test()
        {
            CategoryViewModel category = new CategoryViewModel()
            {
                Id = 1,
                CategoryName = "abc",
                Type = "123",
                Enabled = true
            };

            var efRepository = new EfRepository<Categories>(_dbContext);
            var saveCategoryCommand = new SaveCategoryCommand(efRepository);

            var result = await saveCategoryCommand.ExecuteAsync(category);

            Assert.Equal((int)HttpStatusCode.NotFound, result.GetFirstErrorCode());
        }

        [Fact]
        public async Task SaveCategoryUpdateSuccess_Test()
        {
            Categories category1 = new Categories()
            {
                Id = 1,
                CategoryName = "abc",
                Type = "123",
                Enabled = true
            };
            _dbContext.Set<Categories>().Add(category1);

            CategoryViewModel model = new CategoryViewModel()
            {
                Id = 1,
                CategoryName = "abc123",
                Type = "123",
                Enabled = true
            };

            await _dbContext.SaveChangesAsync();
            var efRepository = new EfRepository<Categories>(_dbContext);
            var saveCategoryCommand = new SaveCategoryCommand(efRepository);
            var result = await saveCategoryCommand.ExecuteAsync(model);

            var getListCategory = new GetCategoryQuery(efRepository);
            var category = (await getListCategory.ExecuteAsync()).FirstOrDefault();

            Assert.Equal(result.Data, model.Id);
            Assert.Equal(model.CategoryName, category.CategoryName);
        }

        [Fact]
        public async Task SaveCategoryInsertSuccess_Test()
        {
            CategoryViewModel model = new CategoryViewModel()
            {
                Id = 0,
                CategoryName = "abc",
                Type = "123",
                Enabled = true
            };

            var efRepository = new EfRepository<Categories>(_dbContext);
            var saveCategoryCommand = new SaveCategoryCommand(efRepository);
            var result = await saveCategoryCommand.ExecuteAsync(model);

            var getListCategory = new GetCategoryQuery(efRepository);
            var category = (await getListCategory.ExecuteAsync()).FirstOrDefault();

            Assert.Equal(result.Data, model.Id);
            Assert.Equal(model.CategoryName, category.CategoryName);
            Assert.Equal(model.Type, category.Type);
            Assert.Equal(model.Enabled, category.Enabled);
        }
    }
}
