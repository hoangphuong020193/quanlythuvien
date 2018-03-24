using Library.Common;
using Library.Data.Entities.Account;
using System;
using System.Collections.Generic;

namespace Library.Data.Entities.Library
{
    public partial class UserBookRequests : IBaseEntity
    {
        public UserBookRequests()
        {
            UserBooks = new HashSet<UserBooks>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string RequestCode { get; set; }
        public DateTime RequestDate { get; set; }

        public Users User { get; set; }
        public ICollection<UserBooks> UserBooks { get; set; }
    }
}
