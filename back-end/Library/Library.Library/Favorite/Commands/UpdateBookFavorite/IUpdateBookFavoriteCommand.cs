using System.Threading.Tasks;

namespace Library.Library.Favorite.Commands.UpdateBookFavorite
{
    public interface IUpdateBookFavoriteCommand
    {
        Task<bool> ExecuteAsync(int bookId);
    }
}
