using System;

namespace Library.Library.Admin.ViewModels
{
    public class ReadStatisticViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int BookId { get; set; }
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int LibraryId { get; set; }
        public string LibraryName { get; set; }
        public int NoOfBorrow { get; set; }
        public int NoOfReturn { get; set; }
        public int Status { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
