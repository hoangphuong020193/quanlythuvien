using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Common;
using Library.Common.Enum;
using Library.Common.Paging;
using Library.Data.Entities.Account;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Queries.GetBookBorrow
{
    public class GetBookBorrowQuery : IGetBookBorrowQuery
    {
        private readonly IRepository<UserBooks> _userBookRepository;
        private readonly HttpContext _httpContext;

        public GetBookBorrowQuery(
            IRepository<UserBooks> userBookRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userBookRepository = userBookRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<PagedList<BookBorrowViewModel>> ExecuteAsync(int page, int pageSize, string status)
        {
            var userId = int.Parse(_httpContext?.User?.UserId());

            if (string.IsNullOrEmpty(status))
            {
                return new PagedList<BookBorrowViewModel>(new List<BookBorrowViewModel>(), page, pageSize, 0);
            }
            var listStatus = status.Split(",").Select(x => int.Parse(x)).ToList();

            await ChangeStatusOfBookOverDeadline(userId);

            List<BookBorrowViewModel> listBooks = new List<BookBorrowViewModel>();

            if (listStatus.Any() && listStatus[0] == 0)
            {
                listBooks = await _userBookRepository.TableNoTracking
                    .Include(x => x.Book)
                    .Include(x => x.Request)
                    .Where(x => x.UserId == userId)
                    .Select(x => new BookBorrowViewModel
                    {
                        BookCode = x.Book.BookCode,
                        BookName = x.Book.BookName,
                        RequestCode = x.Request.RequestCode,
                        RequestDate = x.Request.RequestDate,
                        ReceiveDate = x.ReceiveDate,
                        ReturnDate = x.ReturnDate,
                        Status = x.Status,
                        DeadlineDate = x.DeadlineDate
                    }).OrderByDescending(x => x.DeadlineDate).ToListAsync();
            }
            else
            {
                listBooks = await _userBookRepository.TableNoTracking
                    .Include(x => x.Book)
                    .Include(x => x.Request)
                    .Where(x => x.UserId == userId && listStatus.Contains(x.Status))
                    .Select(x => new BookBorrowViewModel
                    {
                        BookCode = x.Book.BookCode,
                        BookName = x.Book.BookName,
                        RequestCode = x.Request.RequestCode,
                        RequestDate = x.Request.RequestDate,
                        ReceiveDate = x.ReceiveDate,
                        ReturnDate = x.ReturnDate,
                        Status = x.Status,
                        DeadlineDate = x.DeadlineDate
                    }).ToListAsync();
            }

            var result = page == 0 && pageSize == 1 ? listBooks : listBooks.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedList<BookBorrowViewModel>(result, page, pageSize, listBooks.Count());
        }

        private async Task ChangeStatusOfBookOverDeadline(int userId)
        {
            var listBooks = await _userBookRepository.TableNoTracking
                .Where(x => x.UserId == userId && x.Status == (int)BookStatus.Borrowing && DateTime.Now.Date > x.DeadlineDate.Date)
                .ToListAsync();

            listBooks.ForEach(book =>
            {
                book.Status = (int)BookStatus.OutDeadline;
            });

            await _userBookRepository.UpdateAsync(listBooks);
        }
    }
}
