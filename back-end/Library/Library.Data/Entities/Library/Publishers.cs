using Library.Common;
using System.Collections.Generic;

namespace Library.Data.Entities.Library
{
    public partial class Publishers : IBaseEntity
    {
        public Publishers()
        {
            Books = new HashSet<Books>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? Enabled { get; set; }

        public ICollection<Books> Books { get; set; }
    }
}
