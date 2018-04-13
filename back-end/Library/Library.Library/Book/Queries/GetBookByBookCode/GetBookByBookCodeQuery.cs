using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetBookByBookCode
{
    public class GetBookByBookCodeQuery : IGetBookByBookCodeQuery
    {
        private readonly IRepository<Books> _bookRepository;

        public GetBookByBookCodeQuery(IRepository<Books> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookViewModel> ExecuteAsync(string bookCode)
        {
            var result = await _bookRepository.TableNoTracking.Select(x => new BookViewModel
            {
                BookId = x.Id,
                BookCode = x.BookCode,
                BookName = x.BookName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                CategoryType = x.Category.Type,
                Tag = x.Tag,
                Description = x.Description,
                DateImport = x.DateImport.Value,
                Amount = x.Amount.Value,
                AmountAvailable = x.AmountAvailable.Value,
                Author = x.Author,
                PublisherId = x.Publisher != null ? x.PublisherId : 0,
                Publisher = x.Publisher != null ? x.Publisher.Name : "Đang cập nhập",
                SupplierId = x.Supplier != null ? x.SupplierId : 0,
                Supplier = x.Supplier != null ? x.Supplier.Name : "Đang cập nhập",
                Size = x.Size,
                Format = x.Format,
                PublicationDate = x.PublicationDate.Value,
                Pages = x.Pages.Value,
                MaximumDateBorrow = x.MaximumDateBorrow,
                Enabled = x.Enabled.Value
            }).FirstOrDefaultAsync(x => x.BookCode == bookCode);
            return result;
        }
    }
}
