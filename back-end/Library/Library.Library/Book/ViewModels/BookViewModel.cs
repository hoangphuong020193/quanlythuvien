using System;

namespace Library.Library.Book.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string BookCode { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryType { get; set; }
        public string BookName { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public byte[] BookImage { get; set; }
        public DateTime DateImport { get; set; }
        public int Amount { get; set; }
        public int AmountAvailable { get; set; }
        public bool Enabled { get; set; }
        public string Author { get; set; }
        public int PublisherId { get; set; }
        public string Publisher { get; set; }
        public int SupplierId { get; set; }
        public string Supplier { get; set; }
        public int LibraryId { get; set; }
        public string LibraryName { get; set; }
        public string Size { get; set; }
        public string Format { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Pages { get; set; }
        public int MaximumDateBorrow { get; set; }
        public bool Favorite { get; set; }
    }
}
