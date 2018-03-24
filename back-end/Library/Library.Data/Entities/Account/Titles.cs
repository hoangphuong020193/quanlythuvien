using Library.Common;
using System.Collections.Generic;

namespace Library.Data.Entities.Account
{
    public partial class Titles : IBaseEntity
    {
        public Titles()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool? Enabled { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
