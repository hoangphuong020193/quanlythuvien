using System;

namespace Library.Library.Book.ViewModels
{
    public class BookBorrowViewModel
    {
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public string RequestCode { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public int Status { get; set; }
        public int LibraryId { get; set; }
        public string LibraryName { get; set; }
    }
}
