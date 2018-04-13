using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Common.Paging;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Admin.Queries.GetCategoryReport
{
    public class GetCategoryReportQuery : IGetCategoryReportQuery
    {
        private IRepository<Books> _bookRepository;
        private IRepository<Categories> _categoryRepository;

        public GetCategoryReportQuery(
            IRepository<Books> bookRepository,
            IRepository<Categories> categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedList<CategoryReportViewModel>> ExecuteAsync(int page, int pageSize, string searchString)
        {
            var queries = from b in _bookRepository.TableNoTracking
                          join c in _categoryRepository.TableNoTracking on b.CategoryId equals c.Id
                          where c.Enabled.Value
                          select new CategoryReportViewModel
                          {
                              CategoryId = c.Id,
                              CategoryName = c.CategoryName,
                          };

            if (!string.IsNullOrEmpty(searchString))
            {
                queries = queries.Where(x => x.CategoryName.ToLower().Contains(searchString.ToLower()));
            }

            queries = from q in queries
                      group q by new
                      {
                          q.CategoryId
                      } into gsc
                      select new CategoryReportViewModel
                      {
                          CategoryName = gsc.FirstOrDefault().CategoryName,
                          NoOfBook = gsc.Count()
                      };

            var result = page == 0 && pageSize == 1 ? queries : queries.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedList<CategoryReportViewModel>(await result.ToListAsync(), page, pageSize, queries.Count());
        }
    }
}
