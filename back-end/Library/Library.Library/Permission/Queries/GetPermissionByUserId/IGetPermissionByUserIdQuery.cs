using Library.Library.Permission.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Permission.Queries.GetPermissionByUserId
{
    public interface IGetPermissionByUserIdQuery
    {
        Task<List<PermissionViewModel>> ExecuteAsync(int userId = 0);
    }
}
