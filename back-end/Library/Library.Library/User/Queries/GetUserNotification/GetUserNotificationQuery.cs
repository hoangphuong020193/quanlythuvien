using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Common;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.User.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.User.Queries.GetUserNotification
{
    public class GetUserNotificationQuery : IGetUserNotificationQuery
    {
        private readonly IRepository<UserNotifications> _userNotifiRepository;
        private readonly HttpContext _httpContext;

        public GetUserNotificationQuery(
            IRepository<UserNotifications> userNotifiRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userNotifiRepository = userNotifiRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<List<UserNotificationViewModel>> ExecuteAsync()
        {
            int userId = int.Parse(_httpContext?.User?.UserId());

            var result = await _userNotifiRepository.TableNoTracking
                .Where(x => x.UserId == userId)
                .Select(x => new UserNotificationViewModel
                {
                    Message = x.Message,
                    MessageDate = x.MessageDate,
                    IsNew = CheckIsNew(x.MessageDate)
                }).OrderByDescending(x => x.MessageDate).ToListAsync();

            return result;
        }

        private bool CheckIsNew(DateTime messageDate)
        {
            if ((DateTime.Now.Date - messageDate.Date).Days < 2)
            {
                return true;
            }
            return false;
        }
    }
}
