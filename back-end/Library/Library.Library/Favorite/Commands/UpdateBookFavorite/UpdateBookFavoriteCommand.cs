using Library.Common;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Library.Favorite.Commands.UpdateBookFavorite
{
    public class UpdateBookFavoriteCommand : IUpdateBookFavoriteCommand
    {
        private readonly IRepository<BookFavorites> _favoriteRepository;
        private readonly HttpContext _httpContext;

        public UpdateBookFavoriteCommand(
            IRepository<BookFavorites> favoriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _favoriteRepository = favoriteRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public IRepository<BookFavorites> FavoriteRepository => _favoriteRepository;

        public async Task<bool> ExecuteAsync(int bookId)
        {
            int userId = int.Parse(_httpContext?.User?.UserId());
            var bookFavorite = await _favoriteRepository.TableNoTracking.Where(x => x.BookId == bookId && x.UserId == userId).FirstOrDefaultAsync();
            if (bookFavorite != null)
            {
                await _favoriteRepository.DeleteAsync(bookFavorite);
                return false;
            }
            else
            {
                BookFavorites entity = new BookFavorites();
                entity.BookId = bookId;
                entity.UserId = userId;
                await _favoriteRepository.InsertAsync(entity);
                return true;
            }
        }
    }
}
