using Library.Common;
using Library.Data.Entities.Account;
using System;

namespace Library.Data.Entities.Library
{
    public class UserNotifications : IBaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }

        public Users User { get; set; }
    }
}
