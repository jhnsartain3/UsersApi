using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStandardDataAccess<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(string id);
        Task UpdateAsync(string id, TEntity entity);
        Task CreateAsync(TEntity entity);
        Task DeleteAsync(string id);
    }
}