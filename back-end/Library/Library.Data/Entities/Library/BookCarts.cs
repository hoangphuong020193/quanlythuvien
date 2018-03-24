using Library.Common;
using Library.Data.Entities.Account;
using System;

namespace Library.Data.Entities.Library
{
    public partial class BookCarts : IBaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Status { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Books Book { get; set; }
        public Users User { get; set; }
    }
}
