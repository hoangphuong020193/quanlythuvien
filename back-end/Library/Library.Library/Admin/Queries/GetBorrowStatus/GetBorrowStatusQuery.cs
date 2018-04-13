using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Library.Common.Paging;
using Library.Data.Entities.Account;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Admin.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Library.Common.Enum;

namespace Library.Library.Admin.Queries.GetBorrowStatus
{
    public class GetBorrowStatusQuery : IGetBorrowStatusQuery
    {
        private readonly IRepository<UserBooks> _userBookRepository;
        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<UserBookRequests> _requestRepository;
        private readonly IRepository<Libraries> _libraryRepository;

        public GetBorrowStatusQuery(
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

        public async Task<PagedList<BorrowStatusViewModel>> ExecuteAsync(int page, int pageSize, string startDate, string endDate, int libraryId, int status, string searchString)
        {
            DateTime _startDate = DateTime.ParseExact(startDate, "ddMMyyyy", CultureInfo.InvariantCulture);
            DateTime _endDate = DateTime.ParseExact(endDate, "ddMMyyyy", CultureInfo.InvariantCulture);

            var queries = from ub in _userBookRepository.TableNoTracking
                          join u in _userRepository.TableNoTracking on ub.UserId equals u.Id
                          join b in _bookRepository.TableNoTracking on ub.BookId equals b.Id
                          join r in _requestRepository.TableNoTracking on ub.RequestId equals r.Id
                          join lib in _libraryRepository.TableNoTracking on b.LibraryId equals lib.Id
                          where _startDate <= ub.ReceiveDate.Value.Date && ub.ReceiveDate.Value.Date <= _endDate
                          orderby ub.ReceiveDate descending
                          select new BorrowStatusViewModel
                          {
                              UserName = u.UserName,
                              FullName = (u.FirstName + " " + u.MiddleName + " " + u.LastName).Replace("  ", " "),
                              BookCode = b.BookCode,
                              BookName = b.BookName,
                              LibraryId = lib.Id,
                              LibraryName = lib.Name,
                              Status = ub.Status,
                              ReceiveDate = ub.ReceiveDate,
                              ReturnDate = ub.ReturnDate,
                              RequestCode = r.RequestCode
                          };

            if (libraryId != -2)
            {
                queries = queries.Where(x => x.LibraryId == libraryId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                queries = queries.Where(x => x.UserName.ToLower().Contains(searchString)
                 || x.FullName.ToLower().Contains(searchString)
                 || x.BookCode.ToLower() == searchString
                 || x.BookName.ToLower().Contains(searchString)
                 || x.RequestCode.ToLower() == searchString);
            }

            await queries.ForEachAsync((x) =>
            {
                x.Status = x.DeadlineDate.HasValue && x.Status == (int)BookStatus.Borrowing && DateTime.Now.Date > x.DeadlineDate.Value.Date ? (int)BookStatus.OutDeadline : x.Status;
            });

            if (status != -3)
            {
                queries = queries.Where(x => x.Status == status);
            }
            var result = page == 0 && pageSize == 1 ? queries : queries.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedList<BorrowStatusViewModel>(await result.ToListAsync(), page, pageSize, queries.Count());
        }
    }
}
