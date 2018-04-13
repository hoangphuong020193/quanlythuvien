using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetBookPhoto
{
    public class GetBookPhotoQuery : IGetBookPhotoQuery
    {
        private readonly IRepository<Books> _bookRepository;

        public GetBookPhotoQuery(IRepository<Books> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookPhotoViewModel> ExecuteAsync(string code)
        {
            return await _bookRepository.TableNoTracking.Where(x => x.BookCode == code)
                  .Select(x => new BookPhotoViewModel
                  {
                      Content = x.BookImage
                  }).FirstOrDefaultAsync();
        }
    }
}
