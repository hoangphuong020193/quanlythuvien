using Library.Library.BookRequest.ViewModels;
using System.Threading.Tasks;

namespace Library.Library.BookRequest.Queries.GetRequestInfoByCode
{
    public interface IGetRequestInfoByCodeQuery
    {
        Task<BookRequestViewModel> ExecuteAsync(string code);
    }
}
