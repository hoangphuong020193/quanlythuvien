using Library.Library.Category.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Category.Queries.GetCategory
{
    public interface IGetCategoryQuery
    {
        Task<List<CategoryViewModel>> ExecuteAsync();
    }
}
