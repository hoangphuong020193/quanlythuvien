using Library.Library.Book.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetBookPhoto
{
    public interface IGetBookPhotoQuery
    {
        Task<BookPhotoViewModel> ExecuteAsync(string code);
    }
}
