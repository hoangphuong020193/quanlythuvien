using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetBookViewByCategory
{
    public class GetBookViewByCategoryQuery : IGetBookViewByCategoryQuery
    {
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<Categories> _categoryRepository;

        public GetBookViewByCategoryQuery(
            IRepository<Books> bookRepository,
            IRepository<Categories> categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<SearchBookResultViewModel> ExecuteAsync(string view, int page, int pageSize)
        {
            if (string.IsNullOrEmpty(view) || page < 0)
            {
                return new SearchBookResultViewModel();
            }

            IQueryable<BookViewModel> listBooks;
            if (view == "Sách mới")
            {
                listBooks = from book in _bookRepository.TableNoTracking.Where(x => x.Enabled.Value)
                            join cag in _categoryRepository.TableNoTracking on book.CategoryId equals cag.Id
                            orderby book.DateImport descending, book.PublicationDate descending
                            select new BookViewModel
                            {
                                BookId = book.Id,
                                BookCode = book.BookCode,
                                BookName = book.BookName
                            };
            }
            else
            {
                listBooks = from book in _bookRepository.TableNoTracking.Where(x => x.Enabled.Value)
                            join cag in _categoryRepository.TableNoTracking on book.CategoryId equals cag.Id
                            where cag.CategoryName == view
                            orderby book.DateImport descending, book.PublicationDate descending
                            select new BookViewModel
                            {
                                BookId = book.Id,
                                BookCode = book.BookCode,
                                BookName = book.BookName
                            };
            }

            var result = page == 0 && pageSize == 1 ? await listBooks.ToListAsync() : await listBooks.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            SearchBookResultViewModel model = new SearchBookResultViewModel();
            model.Total = await listBooks.CountAsync();
            model.ListBooks = result;

            return model;
        }
    }
}
