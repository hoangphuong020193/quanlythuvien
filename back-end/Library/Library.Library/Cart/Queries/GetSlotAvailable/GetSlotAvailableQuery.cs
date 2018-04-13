using Library.Common;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Library.Cart.Queries.GetSlotAvailable
{
    public class GetSlotAvailableQuery : IGetSlotAvailableQuery
    {
        private readonly IRepository<UserBooks> _userBookRepository;
        private readonly HttpContext _httpContext;

        public GetSlotAvailableQuery(
            IRepository<UserBooks> userBookRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userBookRepository = userBookRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<int> ExecuteAsync()
        {
            var userId = int.Parse(_httpContext?.User?.UserId());
            var result = 10 - (await _userBookRepository.TableNoTracking
                .Where(x => x.UserId == userId && (x.Status == (int)BookStatus.Borrowing || x.Status == (int)BookStatus.Pending))
                .CountAsync());
            return result;
        }
    }
}
