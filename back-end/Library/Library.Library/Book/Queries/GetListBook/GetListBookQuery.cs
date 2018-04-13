using System.Linq;
using System.Threading.Tasks;
using Library.Common.Paging;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetListBook
{
    public class GetListBookQuery : IGetListBookQuery
    {
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<Categories> _categoryRepository;
        private readonly IRepository<Publishers> _publisherRepository;
        private readonly IRepository<Suppliers> _supplierRepository;
        private readonly IRepository<Libraries> _libraryRepository;

        public GetListBookQuery(
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

        public async Task<PagedList<BookViewModel>> ExecuteAsync(int page, int pageSize, string search)
        {
            var books = from book in _bookRepository.TableNoTracking
                          join cat in _categoryRepository.TableNoTracking on book.CategoryId equals cat.Id
                          join pub in _publisherRepository.TableNoTracking on book.PublisherId equals pub.Id
                          join sup in _supplierRepository.TableNoTracking on book.SupplierId equals sup.Id
                          join lib in _libraryRepository.TableNoTracking on book.LibraryId equals lib.Id
                          select new BookViewModel
                          {
                              BookId = book.Id,
                              BookCode = book.BookCode,
                              BookName = book.BookName,
                              CategoryId = cat.Id,
                              CategoryName = cat.CategoryName,
                              CategoryType = cat.Type,
                              Tag = book.Tag,
                              Description = book.Description,
                              DateImport = book.DateImport.Value,
                              Amount = book.Amount.Value,
                              AmountAvailable = book.AmountAvailable.Value,
                              Author = book.Author,
                              PublisherId = pub.Id,
                              Publisher = pub.Name,
                              SupplierId = sup.Id,
                              Supplier = sup.Name,
                              LibraryId = lib.Id,
                              LibraryName = lib.Name,
                              Size = book.Size,
                              Format = book.Format,
                              PublicationDate = book.PublicationDate.Value,
                              Pages = book.Pages.Value,
                              MaximumDateBorrow = book.MaximumDateBorrow,
                              Enabled = book.Enabled.Value
                          };

            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(x => x.BookCode == search || x.BookName.Contains(search));
            }

            var result = page == 0 && pageSize == 1 ? await books.ToListAsync() : await books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<BookViewModel>(result, page, pageSize, books.Count());
        }
    }
}
