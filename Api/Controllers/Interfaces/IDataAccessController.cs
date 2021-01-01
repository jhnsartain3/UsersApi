using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Interfaces
{
    public interface IDataAccessController<TEntity>
    {
        Task<ActionResult<IEnumerable<TEntity>>> GetAll();
        Task<ActionResult<TEntity>> GetById(string id);
        Task<IActionResult> Update(string id, TEntity model);
        Task<ActionResult<TEntity>> Create(TEntity model);
        Task<ActionResult<TEntity>> Delete(string id);
    }
}