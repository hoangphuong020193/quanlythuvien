using System.Linq;
using System.Threading.Tasks;
using Library.Common;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetBookDetail
{
    public class GetBookDetailQuery : IGetBookDetailQuery
    {
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<Publishers> _publisherRepository;
        private readonly IRepository<Suppliers> _supplierRepository;
        private readonly IRepository<Libraries> _libraryRepository;
        private readonly IRepository<BookFavorites> _bookFavoriteRepository;
        private readonly HttpContext _httpContext;

        public GetBookDetailQuery(
            IRepository<Books> bookRepository,
            IRepository<Publishers> publisherRepository,
            IRepository<Suppliers> supplierRepository,
            IRepository<Libraries> libraryRepository,
            IRepository<BookFavorites> bookFavoriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookRepository = bookRepository;
            _publisherRepository = publisherRepository;
            _supplierRepository = supplierRepository;
            _libraryRepository = libraryRepository;
            _bookFavoriteRepository = bookFavoriteRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<BookViewModel> ExecuteAsync(string bookCode)
        {
            var userId = 0;
            int.TryParse(_httpContext?.User?.UserId(), out userId);

            BookViewModel model = new BookViewModel();

            if (userId == 0)
            {
                model = await (from book in _bookRepository.TableNoTracking.Where(x => x.BookCode == bookCode && x.Enabled.Value)
                               join publisher in _publisherRepository.TableNoTracking on book.PublisherId equals publisher.Id into publishers
                               from publisher in publishers.DefaultIfEmpty()
                               join supplier in _supplierRepository.TableNoTracking on book.SupplierId equals supplier.Id into suppliers
                               from supplier in suppliers.DefaultIfEmpty()
                               join lib in _libraryRepository.TableNoTracking on book.LibraryId equals lib.Id into libs
                               from lib in libs.DefaultIfEmpty()
                               select new BookViewModel
                               {
                                   BookId = book.Id,
                                   BookCode = book.BookCode,
                                   BookName = book.BookName,
                                   CategoryId = book.CategoryId,
                                   Tag = book.Tag,
                                   Description = book.Description,
                                   DateImport = book.DateImport.Value,
                                   Amount = book.Amount.Value,
                                   AmountAvailable = book.AmountAvailable.Value,
                                   Author = book.Author,
                                   PublisherId = book.PublisherId,
                                   Publisher = publisher != null ? publisher.Name : "Đang cập nhập",
                                   SupplierId = book.SupplierId,
                                   Supplier = supplier != null ? supplier.Name : "Đang cập nhập",
                                   Size = book.Size,
                                   Format = book.Format,
                                   PublicationDate = book.PublicationDate.Value,
                                   Pages = book.Pages.Value,
                                   MaximumDateBorrow = book.MaximumDateBorrow,
                                   Favorite = false,
                                   LibraryId = book.LibraryId,
                                   LibraryName = lib != null ? lib.Name : "Đang cập nhập",
                                   Enabled = book.Enabled.Value

                               }).FirstOrDefaultAsync();
            }
            else
            {
                model = await (from book in _bookRepository.TableNoTracking.Where(x => x.BookCode == bookCode && x.Enabled.Value)
                               join publisher in _publisherRepository.TableNoTracking on book.PublisherId equals publisher.Id into publishers
                               from publisher in publishers.DefaultIfEmpty()
                               join supplier in _supplierRepository.TableNoTracking on book.SupplierId equals supplier.Id into suppliers
                               from supplier in suppliers.DefaultIfEmpty()
                               join favorite in _bookFavoriteRepository.TableNoTracking.Where(x => x.UserId == userId) on book.Id equals favorite.BookId into favorites
                               from favorite in favorites.DefaultIfEmpty()
                               join lib in _libraryRepository.TableNoTracking on book.LibraryId equals lib.Id into libs
                               from lib in libs.DefaultIfEmpty()
                               select new BookViewModel
                               {
                                   BookId = book.Id,
                                   BookCode = book.BookCode,
                                   BookName = book.BookName,
                                   Tag = book.Tag,
                                   Description = book.Description,
                                   DateImport = book.DateImport.Value,
                                   Amount = book.Amount.Value,
                                   AmountAvailable = book.AmountAvailable.Value,
                                   Author = book.Author,
                                   PublisherId = book.PublisherId,
                                   Publisher = publisher != null ? publisher.Name : "Đang cập nhập",
                                   SupplierId = book.SupplierId,
                                   Supplier = supplier != null ? supplier.Name : "Đang cập nhập",
                                   Size = book.Size,
                                   Format = book.Format,
                                   PublicationDate = book.PublicationDate.Value,
                                   Pages = book.Pages.Value,
                                   MaximumDateBorrow = book.MaximumDateBorrow,
                                   Favorite = favorite != null,
                                   LibraryId = book.LibraryId,
                                   LibraryName = lib != null ? lib.Name : "Đang cập nhập",
                                   Enabled = book.Enabled.Value,
                                   CategoryId = book.CategoryId
                               }).FirstOrDefaultAsync();
            }


            return model;
        }
    }
}
