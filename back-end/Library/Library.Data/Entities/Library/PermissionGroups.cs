using Library.Common;
using System.Collections.Generic;

namespace Library.Data.Entities.Library
{
    public partial class PermissionGroups : IBaseEntity
    {
        public PermissionGroups()
        {
            PermissionGroupMembers = new HashSet<PermissionGroupMembers>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public bool? Enabled { get; set; }

        public ICollection<PermissionGroupMembers> PermissionGroupMembers { get; set; }
    }
}
