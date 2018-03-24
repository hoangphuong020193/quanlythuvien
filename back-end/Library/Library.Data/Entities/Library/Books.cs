using Library.Common;
using System;
using System.Collections.Generic;

namespace Library.Data.Entities.Library
{
    public partial class Books : IBaseEntity
    {
        public Books()
        {
            BookCarts = new HashSet<BookCarts>();
            BookFavorites = new HashSet<BookFavorites>();
            UserBooks = new HashSet<UserBooks>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public byte[] BookImage { get; set; }
        public DateTime? DateImport { get; set; }
        public int? Amount { get; set; }
        public int? AmountAvailable { get; set; }
        public string Author { get; set; }
        public int PublisherId { get; set; }
        public int SupplierId { get; set; }
        public int LibraryId { get; set; }
        public string Size { get; set; }
        public string Format { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int? Pages { get; set; }
        public int MaximumDateBorrow { get; set; }
        public bool? Enabled { get; set; }

        public Categories Category { get; set; }
        public Libraries Library { get; set; }
        public Publishers Publisher { get; set; }
        public Suppliers Supplier { get; set; }
        public ICollection<BookCarts> BookCarts { get; set; }
        public ICollection<BookFavorites> BookFavorites { get; set; }
        public ICollection<UserBooks> UserBooks { get; set; }
    }
}
