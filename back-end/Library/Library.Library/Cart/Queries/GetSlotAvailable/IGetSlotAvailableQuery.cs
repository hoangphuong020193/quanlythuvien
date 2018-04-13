using System.Threading.Tasks;

namespace Library.Library.Cart.Queries.GetSlotAvailable
{
    public interface IGetSlotAvailableQuery
    {
        Task<int> ExecuteAsync();
    }
}
