using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.BookRequest.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.BookRequest.Queries.GetRequestInfoByCode
{
    public class GetRequestInfoByCodeQuery : IGetRequestInfoByCodeQuery
    {
        private readonly IRepository<UserBookRequests> _requestRepository;

        public GetRequestInfoByCodeQuery(IRepository<UserBookRequests> requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<BookRequestViewModel> ExecuteAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            var result = await _requestRepository.TableNoTracking
                .Include(x => x.User)
                .Where(x => x.RequestCode == code)
                .Select(x => new BookRequestViewModel
                {
                    RequestId = x.Id,
                    RequestCode = x.RequestCode,
                    RequestDate = x.RequestDate,
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    FullName = x.User.LastName + " " + x.User.MiddleName + " " + x.User.FirstName
                }).FirstOrDefaultAsync();

            return result;
        }
    }
}
