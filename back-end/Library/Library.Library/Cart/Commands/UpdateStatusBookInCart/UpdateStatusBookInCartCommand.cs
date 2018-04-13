using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Common;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Cart.Commands.UpdateStatusBookInCart
{
    public class UpdateStatusBookInCartCommand : IUpdateStatusBookInCartCommand
    {
        private readonly IRepository<BookCarts> _bookCartRepository;
        private readonly HttpContext _httpContext;

        public UpdateStatusBookInCartCommand(
            IRepository<BookCarts> bookCartRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookCartRepository = bookCartRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<CommandResult> ExecuteAsync(int bookId, int status)
        {
            var userId = int.Parse(_httpContext?.User?.UserId());
            var entity = await _bookCartRepository.TableNoTracking.Where(x => x.BookId == bookId && x.UserId == userId).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Status = status;
                await _bookCartRepository.UpdateAsync(entity);
                return CommandResult.Success;
            }

            return CommandResult.Failed(new CommandResultError
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = "Data not found"
            });
        }
    }
}
