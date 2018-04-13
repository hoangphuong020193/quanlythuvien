using System;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Book.Commands.TakenBook
{
    public class TakenBookCommand : ITakenBookCommand
    {
        private readonly IRepository<UserBooks> _userBookRepository;
        private readonly IRepository<UserNotifications> _notificationRepository;

        public TakenBookCommand(
            IRepository<UserBooks> userBookRepository,
            IRepository<UserNotifications> notificationRepository)
        {
            _userBookRepository = userBookRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<CommandResult> ExecuteAsync(int userId, string bookCode, int requestId)
        {
            try
            {
                var userBook = await _userBookRepository.TableNoTracking
                    .Include(x => x.Book)
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.RequestId == requestId && x.Book.BookCode == bookCode);

                if (userBook == null)
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Description = "Data not found"
                    });
                }

                userBook.Status = (int)BookStatus.Borrowing;
                userBook.ReceiveDate = DateTime.Now;

                if (await _userBookRepository.UpdateAsync(userBook))
                {
                    UserNotifications notify = new UserNotifications();
                    notify.UserId = userBook.UserId;
                    notify.Message = "Bạn đã nhận sách " + userBook.Book.BookName;
                    notify.MessageDate = DateTime.Now;
                    await _notificationRepository.InsertAsync(notify);

                    return CommandResult.Success;
                }

                return CommandResult.Failed();
            }
            catch (Exception ex)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = ex.Message
                });
            }
        }
    }
}
