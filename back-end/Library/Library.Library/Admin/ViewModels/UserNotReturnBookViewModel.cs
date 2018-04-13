using System;

namespace Library.Library.Admin.ViewModels
{
    public class UserNotReturnBookViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int RequestId { get; set; }
        public string RequestCode { get; set; }
        public int BookId { get; set; }
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime DeadlineDate { get; set; }
    }
}
