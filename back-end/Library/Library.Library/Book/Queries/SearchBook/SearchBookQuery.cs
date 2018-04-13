using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.SearchBook
{
    public class SearchBookQuery : ISearchBookQuery
    {
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<Categories> _categoryRepository;
        private readonly IRepository<Publishers> _publisherRepository;
        private readonly IRepository<Suppliers> _supplierRepository;
        private readonly IRepository<Libraries> _libraryRepository;

        public SearchBookQuery(
            IRepository<Books> bookRepository,
            IRepository<Categories> categoryRepository,
            IRepository<Publishers> publisherRepository,
            IRepository<Suppliers> supplierRepository,
            IRepository<Libraries> libraryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
            _supplierRepository = supplierRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<SearchBookResultViewModel> ExecuteAsync(string search, int page, int pageSize)
        {
            if (string.IsNullOrEmpty(search) || page < 0)
            {
                return new SearchBookResultViewModel();
            }

            search = search.Trim().ToLowerInvariant();

            var listBooks = from book in _bookRepository.TableNoTracking.Where(x => x.Enabled.Value)
                            join category in _categoryRepository.TableNoTracking on book.CategoryId equals category.Id
                            join publisher in _publisherRepository.TableNoTracking on book.PublisherId equals publisher.Id into publishers
                            from publisher in publishers.DefaultIfEmpty()
                            join supplier in _supplierRepository.TableNoTracking on book.SupplierId equals supplier.Id into suppliers
                            from supplier in suppliers.DefaultIfEmpty()
                            join lib in _libraryRepository.TableNoTracking on book.LibraryId equals lib.Id into libs
                            from lib in libs.DefaultIfEmpty()
                            where book.BookName.ToLower().Contains(search)
                            || category.CategoryName.ToLower().Contains(search)
                            || book.Author.ToLower().Contains(search)
                            || (publisher != null && publisher.Name.ToLower().Contains(search))
                            || (supplier != null && supplier.Name.ToLower().Contains(search))
                            || (lib != null && lib.Name.ToLower().Contains(search))
                            orderby book.DateImport descending, book.PublicationDate descending
                            select new BookViewModel
                            {
                                BookId = book.Id,
                                BookCode = book.BookCode,
                                BookName = book.BookName
                            };

            var result = page == 0 && pageSize == 1 ? await listBooks.ToListAsync() : await listBooks.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            SearchBookResultViewModel model = new SearchBookResultViewModel();
            model.Total = await listBooks.CountAsync();
            model.ListBooks = result;

            return model;
        }
    }
}
