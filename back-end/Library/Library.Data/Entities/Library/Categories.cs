using Library.Common;
using System.Collections.Generic;

namespace Library.Data.Entities.Library
{
    public partial class Categories : IBaseEntity
    {
        public Categories()
        {
            Books = new HashSet<Books>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string CategoryName { get; set; }
        public bool? Enabled { get; set; }

        public ICollection<Books> Books { get; set; }
    }
}
