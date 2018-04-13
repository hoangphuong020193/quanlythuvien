using System;
using System.Linq;
using System.Threading.Tasks;
using Library.Common.Enum;
using Library.Common.Paging;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Admin.Queries.GetListUserNotReturnBook
{
    public class GetListUserNotReturnBookQuery : IGetListUserNotReturnBookQuery
    {
        private readonly IRepository<UserBooks> _userBookRepository;

        public GetListUserNotReturnBookQuery(IRepository<UserBooks> userBookRepository)
        {
            _userBookRepository = userBookRepository;
        }

        public async Task<PagedList<UserNotReturnBookViewModel>> ExecuteAsync(int page, int pageSize)
        {
            var listUserBooks = _userBookRepository.TableNoTracking
                .Include(x => x.Request)
                .Include(x => x.Book)
                .Include(x => x.User)
                .Where(x => (x.Status == (int)BookStatus.Borrowing && x.DeadlineDate < DateTime.Now.Date) || x.Status == (int)BookStatus.OutDeadline)
                .Select(x => new UserNotReturnBookViewModel()
                {
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    FirstName = x.User.FirstName,
                    MiddleName = x.User.MiddleName,
                    LastName = x.User.LastName,
                    RequestId = x.RequestId,
                    RequestCode = x.Request.RequestCode,
                    BookId = x.BookId,
                    BookCode = x.Book.BookCode,
                    BookName = x.Book.BookName,
                    ReceivedDate = x.ReceiveDate.Value,
                    DeadlineDate = x.DeadlineDate
                });

            var result = page == 0 && pageSize == 1 ? listUserBooks : listUserBooks.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedList<UserNotReturnBookViewModel>(await result.ToListAsync(), page, pageSize, listUserBooks.Count());

        }
    }
}
