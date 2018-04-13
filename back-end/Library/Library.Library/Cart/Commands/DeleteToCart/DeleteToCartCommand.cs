using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Common;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Cart.Commands.DeleteToCart
{
    public class DeleteToCartCommand : IDeleteToCartCommand
    {
        private readonly IRepository<BookCarts> _bookCartRepository;
        private readonly HttpContext _httpContext;

        public DeleteToCartCommand(
            IRepository<BookCarts> bookCartRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookCartRepository = bookCartRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<CommandResult> ExecuteAsync(int bookId)
        {
            var userId = int.Parse(_httpContext?.User?.UserId());

            BookCarts entity = await _bookCartRepository.TableNoTracking
                .Where(x => x.UserId == userId && x.BookId == bookId).FirstOrDefaultAsync();

            if (entity == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = "Data not found"
                });
            }

            await _bookCartRepository.DeleteAsync(entity);
            return CommandResult.Success;
        }
    }
}
