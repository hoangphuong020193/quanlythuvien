using Library.Library.Library.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Library.Queries.GetListLibrary
{
    public interface IGetListLibraryQuery
    {
        Task<List<LibraryViewModel>> ExecuteAsync();
    }
}
