using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Common;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetListNewBook
{
    public class GetListBookNewQuery : IGetListBookNewQuery
    {
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<BookFavorites> _bookFavoriteRepository;
        private readonly HttpContext _httpContext;

        public GetListBookNewQuery(
            IRepository<Books> bookRepository,
            IRepository<BookFavorites> bookFavoriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookRepository = bookRepository;
            _bookFavoriteRepository = bookFavoriteRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<List<BookViewModel>> ExecuteAsync()
        {
            var userId = 0;
            int.TryParse(_httpContext?.User?.UserId(), out userId);

            List<BookViewModel> model = new List<BookViewModel>();

            if (userId == 0)
            {
                model = await _bookRepository.TableNoTracking.Where(x => x.Enabled.Value)
                    .OrderByDescending(x => x.DateImport).Take(10)
                    .Select(x => new BookViewModel
                    {
                        BookId = x.Id,
                        BookCode = x.BookCode,
                        BookName = x.BookName
                    }).ToListAsync();
            }
            else
            {
                model = await (from book in _bookRepository.TableNoTracking.Where(x => x.Enabled.Value)
                               join favorite in _bookFavoriteRepository.TableNoTracking.Where(x => x.UserId == userId) on book.Id equals favorite.BookId into favorites
                               from favorite in favorites.DefaultIfEmpty()
                               orderby book.DateImport
                               select new BookViewModel
                               {
                                   BookId = book.Id,
                                   BookCode = book.BookCode,
                                   BookName = book.BookName,
                                   Favorite = favorite != null
                               }).ToListAsync();
            }
            return model;
        }
    }
}
