using System;

namespace Library.Library.Cart.ViewModels
{
    public class BookInCartDetailViewModel
    {
        public int BookId { get; set; }
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Amoun { get; set; }
        public int AmountAvailable { get; set; }
        public int Status { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int MaximumDateBorrow { get; set; }
        public DateTime ReturnDate { get; set; }
        public string LibraryName { get; set; }
    }
}
