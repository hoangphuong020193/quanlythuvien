using System;

namespace Library.Library.Admin.ViewModels
{
    public class BorrowStatusViewModel
    {
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int Status { get; set; }
        public string RequestCode { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public int LibraryId { get; set; }
        public string LibraryName { get; set; }
    }
}
