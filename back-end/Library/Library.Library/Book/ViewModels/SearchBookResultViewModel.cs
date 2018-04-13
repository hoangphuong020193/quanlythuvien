using System.Collections.Generic;

namespace Library.Library.Book.ViewModels
{
    public class SearchBookResultViewModel
    {
        public int Total { get; set; }
        public List<BookViewModel> ListBooks { get; set; }
    }
}
