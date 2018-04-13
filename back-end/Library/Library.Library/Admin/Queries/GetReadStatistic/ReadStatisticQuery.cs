using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Library.Common.Paging;
using Library.Data.Entities.Account;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Admin.Queries.GetReadStatistic
{
    public class ReadStatisticQuery : IReadStatisticQuery
    {
        private readonly IRepository<UserBooks> _userBookRepository;
        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<Libraries> _libraryRepository;
        private readonly IRepository<Categories> _categoryRepository;

        private const int BOOK = 0;
        private const int CATEGORY = 1;
        private const int USER = 2;
        private const int LIBRARY = 3;

        public ReadStatisticQuery(
            IRepository<UserBooks> userBookRepository,
            IRepository<Users> userRepository,
            IRepository<Books> bookRepository,
            IRepository<Libraries> libraryRepository,
            IRepository<Categories> categoryRepository)
        {
            _userBookRepository = userBookRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedList<ReadStatisticViewModel>> ExecuteAsync(int page, int pageSize, string startDate, string endDate, int groupBy)
        {
            DateTime _startDate = DateTime.ParseExact(startDate, "ddMMyyyy", CultureInfo.InvariantCulture);
            DateTime _endDate = DateTime.ParseExact(endDate, "ddMMyyyy", CultureInfo.InvariantCulture);

            var queries = from ub in _userBookRepository.TableNoTracking
                          join u in _userRepository.TableNoTracking on ub.UserId equals u.Id
                          join b in _bookRepository.TableNoTracking on ub.BookId equals b.Id
                          join lib in _libraryRepository.TableNoTracking on b.LibraryId equals lib.Id
                          join c in _categoryRepository.TableNoTracking on b.CategoryId equals c.Id
                          where _startDate <= ub.ReceiveDate.Value.Date && ub.ReceiveDate.Value.Date <= _endDate
                          orderby ub.ReceiveDate descending
                          select new ReadStatisticViewModel
                          {
                              UserId = u.Id,
                              UserName = u.UserName,
                              FirstName = u.FirstName,
                              MiddleName = u.MiddleName,
                              LastName = u.LastName,
                              BookId = b.Id,
                              BookCode = b.BookCode,
                              BookName = b.BookName,
                              CategoryId = c.Id,
                              CategoryName = c.CategoryName,
                              LibraryId = lib.Id,
                              LibraryName = lib.Name,
                              Status = ub.Status,
                              ReceiveDate = ub.ReceiveDate,
                              ReturnDate = ub.ReturnDate
                          };

            switch (groupBy)
            {
                case BOOK:
                    queries = from q in queries
                              group q by new
                              {
                                  q.BookId,
                                  q.BookCode
                              } into gsc
                              select new ReadStatisticViewModel
                              {
                                  BookCode = gsc.Key.BookCode,
                                  BookName = gsc.FirstOrDefault().BookName,
                                  NoOfBorrow = gsc.Count(x => x.ReceiveDate.HasValue),
                                  NoOfReturn = gsc.Count(x => x.ReturnDate.HasValue)
                              };
                    break;
                case CATEGORY:
                    queries = from q in queries
                              group q by new
                              {
                                  q.CategoryId,
                              } into gsc
                              select new ReadStatisticViewModel
                              {
                                  CategoryName = gsc.FirstOrDefault().CategoryName,
                                  NoOfBorrow = gsc.Count(x => x.ReceiveDate.HasValue),
                                  NoOfReturn = gsc.Count(x => x.ReturnDate.HasValue),
                              };
                    break;
                case USER:
                    queries = from q in queries
                              group q by new
                              {
                                  q.UserId,
                              } into gsc
                              select new ReadStatisticViewModel
                              {
                                  UserName = gsc.FirstOrDefault().UserName,
                                  FirstName = gsc.FirstOrDefault().FirstName,
                                  MiddleName = gsc.FirstOrDefault().MiddleName,
                                  LastName = gsc.FirstOrDefault().LastName,
                                  NoOfBorrow = gsc.Count(x => x.ReceiveDate.HasValue),
                                  NoOfReturn = gsc.Count(x => x.ReturnDate.HasValue),
                              };
                    break;
                case LIBRARY:
                    queries = from q in queries
                              group q by new
                              {
                                  q.LibraryId,
                              } into gsc
                              select new ReadStatisticViewModel
                              {
                                  LibraryName = gsc.FirstOrDefault().LibraryName,
                                  NoOfBorrow = gsc.Count(x => x.ReceiveDate.HasValue),
                                  NoOfReturn = gsc.Count(x => x.ReturnDate.HasValue),
                              };
                    break;
            }

            var result = page == 0 && pageSize == 1 ? queries : queries.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedList<ReadStatisticViewModel>(await result.ToListAsync(), page, pageSize, queries.Count());
        }
    }
}
