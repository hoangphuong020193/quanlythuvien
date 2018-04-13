using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Common;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Cart.Commands.BorrowBook
{
    public class BorrowBookCommand : IBorrowBookCommand
    {
        private readonly IRepository<BookCarts> _bookCartRepository;
        private readonly IRepository<UserBooks> _userBookRepository;
        private readonly IRepository<Books> _bookRepository;
        private readonly IRepository<UserBookRequests> _bookRequestRepository;
        private readonly HttpContext _httpContext;

        private static Random random = new Random();

        public BorrowBookCommand(
            IRepository<BookCarts> bookCartRepository,
            IRepository<UserBooks> userBookRepository,
            IRepository<Books> bookRepository,
            IRepository<UserBookRequests> bookRequestRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookCartRepository = bookCartRepository;
            _userBookRepository = userBookRepository;
            _bookRepository = bookRepository;
            _bookRequestRepository = bookRequestRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<CommandResult> ExecuteAsync()
        {
            var userId = int.Parse(_httpContext?.User?.UserId());

            var slotAvaiable = 10 - (await _userBookRepository.TableNoTracking
                .Where(x => x.UserId == userId && (x.Status == (int)BookStatus.Borrowing || x.Status == (int)BookStatus.Pending))
                .CountAsync());

            if (slotAvaiable <= 0)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.LengthRequired,
                    Description = "Out of slot"
                });
            }

            var bookCart = await _bookCartRepository.TableNoTracking.Where(x => x.UserId == userId && x.Status == (int)BookStatus.InOrder).ToListAsync();

            var listBookBorrow = await _bookRepository.TableNoTracking.Where(x => bookCart.Select(y => y.BookId).Contains(x.Id)).ToListAsync();

            if (listBookBorrow.Count() > slotAvaiable || listBookBorrow.Any(x => x.AmountAvailable == 0))
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.LengthRequired,
                    Description = "Out of slot"
                });
            }

            var listEnity = listBookBorrow.Select(x => new UserBooks
            {
                UserId = userId,
                BookId = x.Id,
                Status = (int)BookStatus.Pending,
                DeadlineDate = DateTime.Now.Date.AddDays(x.MaximumDateBorrow)
            }).ToList();

            UserBookRequests request = new UserBookRequests();
            request.UserId = userId;
            request.RequestDate = DateTime.Now;
            request.RequestCode = GenerationCode();
            request.UserBooks = listEnity;

            if (await _bookRequestRepository.InsertAsync(request))
            {
                listBookBorrow.ForEach(book =>
                {
                    book.AmountAvailable = book.AmountAvailable - 1;
                });

                bookCart.ForEach(item =>
                {
                    item.Status = (int)BookStatus.Borrowing;
                });

                await _bookRepository.UpdateAsync(listBookBorrow);
                await _bookCartRepository.UpdateAsync(bookCart);

                return CommandResult.SuccessWithData(request.RequestCode);
            }

            return CommandResult.Failed();
        }


        public string GenerationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
