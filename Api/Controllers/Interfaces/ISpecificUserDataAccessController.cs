﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers.Interfaces
{
    public interface ISpecificUserDataAccessController<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(string userId);
        Task<TEntity> GetByIdAsync(string userId, string id);
        Task UpdateAsync(string userId, string id, TEntity entity);
        Task CreateAsync(string userId, TEntity entity);
        Task DeleteAsync(string userId, string id);
    }
}