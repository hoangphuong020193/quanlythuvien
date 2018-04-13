using Library.Library.Book.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Book.Queries.GetBookSection
{
    public interface IGetBookSectionQuery
    {
        Task<List<BookViewModel>> ExecuteAsync(string sectionType, int take = 10);
    }
}
