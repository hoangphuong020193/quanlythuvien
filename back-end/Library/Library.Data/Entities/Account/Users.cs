using Library.Common;
using Library.Data.Entities.Library;
using System;
using System.Collections.Generic;

namespace Library.Data.Entities.Account
{
    public partial class Users : IBaseEntity
    {
        public Users()
        {
            BookCarts = new HashSet<BookCarts>();
            BookFavorites = new HashSet<BookFavorites>();
            PermissionGroupMembers = new HashSet<PermissionGroupMembers>();
            UserBookRequests = new HashSet<UserBookRequests>();
            UserBooks = new HashSet<UserBooks>();
            UserNotifications = new HashSet<UserNotifications>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SchoolEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public int TitleId { get; set; }
        public DateTime? JoinDate { get; set; }
        public bool? Enabled { get; set; }

        public Titles Title { get; set; }
        public ICollection<BookCarts> BookCarts { get; set; }
        public ICollection<BookFavorites> BookFavorites { get; set; }
        public ICollection<PermissionGroupMembers> PermissionGroupMembers { get; set; }
        public ICollection<UserBookRequests> UserBookRequests { get; set; }
        public ICollection<UserBooks> UserBooks { get; set; }
        public ICollection<UserNotifications> UserNotifications { get; set; }
    }
}
