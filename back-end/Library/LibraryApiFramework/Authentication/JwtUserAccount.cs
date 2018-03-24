namespace Library.ApiFramework.Authentication
{
    public class JwtUserAccount
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
    }
}
