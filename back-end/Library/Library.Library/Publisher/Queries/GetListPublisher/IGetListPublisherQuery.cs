using Library.Library.Publisher.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Library.Publisher.Queries.GetListPublisher
{
    public interface IGetListPublisherQuery
    {
        Task<List<PublisherViewModel>> ExecuteAsync();
    }
}
