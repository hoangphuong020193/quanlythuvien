using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Common;
using Library.Common.Enum;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Cart.Commands.AddBookToCart
{
    public class AddBookToCartCommand : IAddBookToCartCommand
    {
        private readonly IRepository<BookCarts> _bookCartRepository;
        private readonly HttpContext _httpContext;

        public AddBookToCartCommand(
            IRepository<BookCarts> bookCartRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookCartRepository = bookCartRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<CommandResult> ExecuteAsync(List<int> bookIds)
        {
            var userId = int.Parse(_httpContext?.User?.UserId());

            List<int> listBooKInCartExisted = await _bookCartRepository.TableNoTracking
                .Where(x => x.UserId == userId && bookIds.Contains(x.BookId) && (x.Status == (int)BookStatus.InOrder || x.Status == (int)BookStatus.Waiting))
                .Select(x => x.BookId)
                .ToListAsync();

            List<int> listBooKInCartNew = bookIds.Where(x => !listBooKInCartExisted.Contains(x)).ToList();

            if (!listBooKInCartNew.Any())
            {
                return CommandResult.Success;
            }

            List<BookCarts> entities = new List<BookCarts>();
            listBooKInCartNew.ForEach(x =>
            {
                BookCarts entity = new BookCarts();
                entity.BookId = x;
                entity.UserId = userId;
                entity.Status = (int)BookStatus.InOrder;
                entities.Add(entity);
            });
            await _bookCartRepository.InsertAsync(entities);

            return CommandResult.SuccessWithData(entities);
        }
    }
}
