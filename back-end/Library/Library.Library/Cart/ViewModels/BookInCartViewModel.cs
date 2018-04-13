using System;

namespace Library.Library.Cart.ViewModels
{
    public class BookInCartViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Status { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
