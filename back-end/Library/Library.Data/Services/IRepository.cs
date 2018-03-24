using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Data.Services
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
        Task<T> GetByIdAsync(object id);
        Task<bool> InsertAsync(T entity);
        Task<bool> InsertAsync(IEnumerable<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateAsync(IEnumerable<T> entities);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(IEnumerable<T> entities);
        Task<bool> ExecuteSqlCommandAsync(string sql, params object[] parameters);

        Task<List<Dictionary<string, object>>> ExecuteStoredProcedureListAsync(string commandText,
            params object[] parameters);

        string TableName { get; }
    }
}
