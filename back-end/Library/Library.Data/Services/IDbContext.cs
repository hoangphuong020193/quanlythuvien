using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Data.Services
{
    public interface IDbContext
    {
        IModel Model { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
        int SaveChanges();
        Task<int> ExecuteSqlCommandAsync(string commandText, params object[] parameters);

        Task<List<Dictionary<string, object>>> ExecuteStoredProcedureListAsync(string commandText,
            params object[] parameters);
    }
}
