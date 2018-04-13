using Library.Data.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTest.DataTests
{
    public class ApplicationDbContextTest
    {
        [Fact]
        public void InitDbContext_test()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            options.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=library;Trusted_Connection=True;");

            var ex = Record.Exception(() =>
            {
                var applicationDbContext = new ApplicationDbContext(options.Options);
            });
            Assert.Null(ex);
        }
    }
}
