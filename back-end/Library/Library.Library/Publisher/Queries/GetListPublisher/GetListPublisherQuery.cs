using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Publisher.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Publisher.Queries.GetListPublisher
{
    public class GetListPublisherQuery : IGetListPublisherQuery
    {
        private readonly IRepository<Publishers> _publisherRepository;

        public GetListPublisherQuery(IRepository<Publishers> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public async Task<List<PublisherViewModel>> ExecuteAsync()
        {
            var result = await _publisherRepository.TableNoTracking.Select(x => new PublisherViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email,
                Enabled = x.Enabled.Value
            }).ToListAsync();

            return result;
        }
    }
}
