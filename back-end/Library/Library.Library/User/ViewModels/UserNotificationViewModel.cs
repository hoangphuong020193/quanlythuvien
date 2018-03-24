using System;

namespace Library.Library.User.ViewModels
{
    public class UserNotificationViewModel
    {
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
        public bool IsNew { get; set; }
    }
}
