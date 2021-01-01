using System.Collections.Generic;
using System.Threading.Tasks;

namespace Consumables.Interfaces
{
    public interface IStandardConsumable<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(string id);
        Task UpdateAsync(string id, TEntity entity);
        Task CreateAsync(TEntity entity);
        Task DeleteAsync(string id);
    }
}