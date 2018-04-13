using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Common;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Cart.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Cart.Queries.GetListBookInCart
{
    public class GetListBookInCartQuery : IGetListBookInCartQuery
    {
        private readonly IRepository<BookCarts> _bookCartRepository;
        private readonly HttpContext _httpContext;

        public GetListBookInCartQuery(
            IRepository<BookCarts> bookCartRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookCartRepository = bookCartRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<List<BookInCartViewModel>> ExecuteAsync()
        {
            var userId = int.Parse(_httpContext?.User?.UserId());

            var result = await _bookCartRepository.TableNoTracking
                .Where(x => x.UserId == userId && (x.Status == (int)BookStatus.InOrder || x.Status == (int)BookStatus.Waiting))
                .Select(x => new BookInCartViewModel
                {
                    Id = x.Id,
                    BookId = x.BookId,
                    Status = x.Status,

                }).ToListAsync();

            return result;
        }
    }
}
