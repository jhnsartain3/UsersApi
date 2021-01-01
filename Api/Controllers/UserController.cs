using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers.Base;
using Api.Controllers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController, IDataAccessController<UserModel>
    {
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        {
            LoggerWrapper.LogInformation("Get All Users Models", GetType().Name, nameof(GetAll), null);

            return Ok(await UserService.GetAllAsync());
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetById(string id)
        {
            LoggerWrapper.LogInformation($"Get User Model By Id: {id}", GetType().Name, nameof(GetById), null);

            return Ok(await UserService.GetByIdAsync(id));
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserModel model)
        {
            LoggerWrapper.LogInformation($"Update User Model: {id}", GetType().Name, nameof(Update), null);

            await UserService.UpdateAsync(id, model);

            return NoContent();
        }

        // POST: api/User
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserModel>> Create(UserModel model)
        {
            LoggerWrapper.LogInformation($"Create User Model: {model.Username}", GetType().Name, nameof(Create), null);

            await UserService.CreateAsync(model);

            return Created(nameof(Create), new {id = model.Id});
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserModel>> Delete(string id)
        {
            LoggerWrapper.LogInformation($"Delete User Model: {id}", GetType().Name, nameof(Delete), null);

            await UserService.DeleteAsync(id);

            return NoContent();
        }

        // GET: api/User/GetQuantityOfUsers
        [HttpGet("GetQuantityOfUsers")]
        public async Task<ActionResult<int>> GetQuantityOfUsers()
        {
            LoggerWrapper.LogInformation("Get quantity of user models", GetType().Name, nameof(GetQuantityOfUsers),
                null);

            return Ok(await UserService.GetQuantityOfUsersAsync());
        }

        // POST: api/User/UsernameExists/John
        [HttpPost("UsernameExists/{username}")]
        public async Task<ActionResult<bool>> Exists(string username)
        {
            LoggerWrapper.LogInformation($"User Model exists with name: {username}", GetType().Name, nameof(Exists),
                null);

            return Ok(await UserService.UsernameExistsAsync(username));
        }

        // POST: api/User/CredentialsValid
        [HttpPost("CredentialsValid")]
        public async Task<ActionResult<bool>> AreCredentialsValid(UserModel model)
        {
            LoggerWrapper.LogInformation($"Credentials are valid: {model.Username}", GetType().Name,
                nameof(AreCredentialsValid), null);

            return Ok(await UserService.AreCredentialsValidAsync(model));
        }
    }
}