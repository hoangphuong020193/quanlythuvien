using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Library.Common.Enum;
using Library.Common.Paging;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.BookBorrowAmount
{
    public class BookBorrowAmountQuery : IBookBorrowAmountQuery
    {
        private readonly IRepository<UserBooks> _userBookRepository;

        public BookBorrowAmountQuery(
            IRepository<UserBooks> userBookRepository)
        {
            _userBookRepository = userBookRepository;
        }

        public async Task<PagedList<BookBorrowAmountViewModel>> ExecuteAsync(int page, int pageSize, string startDate, string endDate)
        {
            DateTime _startDate = DateTime.ParseExact(startDate, "ddMMyyyy", CultureInfo.InvariantCulture);
            DateTime _endDate = DateTime.ParseExact(endDate, "ddMMyyyy", CultureInfo.InvariantCulture);
            var queries = _userBookRepository.TableNoTracking
                .Include(x => x.Book)
                .Where(x => _startDate.Date <= x.ReceiveDate.Value.Date && x.ReceiveDate.Value.Date <= _endDate.Date
                && (x.Status == (int)BookStatus.Borrowing
                || x.Status == (int)BookStatus.Returned
                || x.Status == (int)BookStatus.OutDeadline));

            var groupQ = (from q in queries
                          group q by new
                          {
                              q.BookId
                          } into gsc
                          select new BookBorrowAmountViewModel
                          {
                              BookCode = gsc.FirstOrDefault().Book.BookCode,
                              BookName = gsc.FirstOrDefault().Book.BookName,
                              Amount = gsc.Count()
                          }).OrderByDescending(x => x.Amount);

            var result = page == 0 && pageSize == 1 ? groupQ : groupQ.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedList<BookBorrowAmountViewModel>(await result.ToListAsync(), page, pageSize, result.Count());

        }

    }
}
