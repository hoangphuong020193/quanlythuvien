using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Library.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Library.Queries.GetListLibrary
{
    public class GetListLibraryQuery : IGetListLibraryQuery
    {
        private readonly IRepository<Libraries> _libraryRepository;

        public GetListLibraryQuery(IRepository<Libraries> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<List<LibraryViewModel>> ExecuteAsync()
        {
            var result = await _libraryRepository.TableNoTracking.Select(x => new LibraryViewModel
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
