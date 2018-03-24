using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Library.Permission.ViewModels;
using Library.Data.Services;
using Library.Data.Entities.Library;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Library.Common;

namespace Library.Library.Permission.Queries.GetPermissionByUserId
{
    public class GetPermissionByUserIdQuery : IGetPermissionByUserIdQuery
    {
        private readonly IRepository<PermissionGroups> _permissionRepository;
        private readonly IRepository<PermissionGroupMembers> _permissionMemberRepository;
        private readonly HttpContext _httpContext;

        public GetPermissionByUserIdQuery(
            IRepository<PermissionGroups> permissionRepository,
            IRepository<PermissionGroupMembers> permissionMemberRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _permissionRepository = permissionRepository;
            _permissionMemberRepository = permissionMemberRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<List<PermissionViewModel>> ExecuteAsync(int userId = 0)
        {
            if (userId == 0)
            {
                userId = int.Parse(_httpContext?.User?.UserId());
            }

            var groups = (from permission in _permissionRepository.TableNoTracking.AsQueryable()
                          join permissionMember in _permissionMemberRepository.TableNoTracking.AsQueryable()
                          on permission.Id equals permissionMember.GroupId
                          where permission.Enabled == true && permissionMember.UserId == userId
                          select new PermissionViewModel
                          {
                              GroupPermissionId = permission.Id,
                              GroupPermissionName = permission.GroupName
                          });

            return await groups.GroupBy(p => p.GroupPermissionId).Select(pg => pg.First()).ToListAsync();
        }
    }
}
