using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Category.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Category.Queries.GetCategory
{
    public class GetCategoryQuery : IGetCategoryQuery
    {
        private readonly IRepository<Categories> _categoryRepository;

        public GetCategoryQuery(IRepository<Categories> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryViewModel>> ExecuteAsync()
        {
            var result = await _categoryRepository.TableNoTracking.Where(x => x.Enabled.Value)
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName,
                    Type = x.Type,
                    Enabled = x.Enabled.Value
                }).OrderBy(x => x.Type).ToListAsync();

            return result;
        }
    }
}
