using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetBookSection
{
    public class GetBookSectionQuery : IGetBookSectionQuery
    {
        private readonly IRepository<Books> _bookRepository;

        public GetBookSectionQuery(IRepository<Books> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookViewModel>> ExecuteAsync(string sectionType, int take = 10)
        {
            List<BookViewModel> model = await _bookRepository.TableNoTracking
                .Include(x => x.Category)
                .Where(x => x.Enabled.Value && x.Category.CategoryName.ToLower() == sectionType.ToLower())
                .OrderByDescending(x => x.DateImport)
                .Take(take)
                .Select(x => new BookViewModel
                {
                    BookId = x.Id,
                    BookCode = x.BookCode,
                    BookName = x.BookName
                }).ToListAsync();

            return model;
        }
    }
}
