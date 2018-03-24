using Library.Common;
using Library.Data.Entities.Account;

namespace Library.Data.Entities.Library
{
    public partial class BookFavorites : IBaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        public Books Book { get; set; }
        public Users User { get; set; }
    }
}
