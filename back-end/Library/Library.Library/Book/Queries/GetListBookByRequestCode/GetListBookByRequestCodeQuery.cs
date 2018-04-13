using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Common.Enum;
using Library.Data.Entities.Account;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetListBookByRequestCode
{
    public class GetListBookByRequestCodeQuery : IGetListBookByRequestCodeQuery
    {
        private readonly IRepository<UserBooks> _userBookRepository;
        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<UserBookRequests> _requestRepository;
        private readonly IRepository<Libraries> _libraryRepository;

        public GetListBookByRequestCodeQuery(
            IRepository<UserBooks> userBookRepository,
            IRepository<Users> userRepository,
            IRepository<Books> bookRepository,
            IRepository<UserBookRequests> requestRepository,
            IRepository<Libraries> libraryRepository)
        {
            _userBookRepository = userBookRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _requestRepository = requestRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<List<BookBorrowViewModel>> ExecuteAsync(string code, int libraryId = -3)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new List<BookBorrowViewModel>();
            }

            var result = (from ub in _userBookRepository.TableNoTracking
                          join b in _bookRepository.TableNoTracking on ub.BookId equals b.Id
                          join u in _userRepository.TableNoTracking on ub.UserId equals u.Id
                          join r in _requestRepository.TableNoTracking on ub.RequestId equals r.Id
                          join l in _libraryRepository.TableNoTracking on b.LibraryId equals l.Id
                          where (r.RequestCode == code || u.UserName == code) && (ub.Status == (int)BookStatus.Pending || ub.Status == (int)BookStatus.Borrowing || ub.Status == (int)BookStatus.Returned)
                          select new BookBorrowViewModel
                          {
                              BookCode = b.BookCode,
                              BookName = b.BookName,
                              RequestCode = r.RequestCode,
                              RequestDate = r.RequestDate,
                              ReceiveDate = ub.ReceiveDate,
                              ReturnDate = ub.ReturnDate,
                              Status = ub.Status,
                              DeadlineDate = ub.DeadlineDate,
                              LibraryId = l.Id,
                              LibraryName = l.Name
                          });

            if (libraryId != -3)
            {
                result = result.Where(x => x.LibraryId == libraryId);
            }

            return await result.OrderByDescending(x => x.DeadlineDate).ToListAsync();
        }
    }
}
