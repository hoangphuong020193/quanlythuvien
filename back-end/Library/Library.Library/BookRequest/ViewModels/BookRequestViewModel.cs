using System;

namespace Library.Library.BookRequest.ViewModels
{
    public class BookRequestViewModel
    {
        public int RequestId { get; set; }
        public string RequestCode { get; set; }
        public DateTime RequestDate { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
    }
}
