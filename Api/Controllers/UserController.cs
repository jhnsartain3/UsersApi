using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers.Base;
using Api.Controllers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sartain_Studios_Common.SharedEntities;
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
        [Authorize(Roles = Role.Service + "," + Role.Administrator)]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        {
            LoggerWrapper.LogInformation("Get All Users Models", GetType().Name, nameof(GetAll), null);

            return Ok(await UserService.GetAllAsync(GetUserIdFromRequestHeaders()));
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [Authorize(Roles = Role.Service + "," + Role.User + "," + Role.Administrator)]
        public async Task<ActionResult<UserModel>> GetById(string id)
        {
            LoggerWrapper.LogInformation($"Get User Model By Id: {id}", GetType().Name, nameof(GetById), null);

            return Ok(await UserService.GetByIdAsync(id, null));
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [Authorize(Roles = Role.Service + "," + Role.User + "," + Role.Administrator)]
        public async Task<IActionResult> Update(string id, UserModel model)
        {
            LoggerWrapper.LogInformation($"Update User Model: {id}", GetType().Name, nameof(Update), null);

            await UserService.UpdateAsync(GetUserIdFromRequestHeaders(), id, model);

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserModel>> Create(UserModel model)
        {
            LoggerWrapper.LogInformation($"Create User Model: {model.Username}", GetType().Name, nameof(Create), null);

            await UserService.CreateAsync(null, model);

            return Created(nameof(Create), new { id = model.Id });
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Service + "," + Role.Administrator + "," + Role.God)]
        public async Task<ActionResult<UserModel>> Delete(string id)
        {
            LoggerWrapper.LogInformation($"Delete User Model: {id}", GetType().Name, nameof(Delete), null);

            await UserService.DeleteAsync(GetUserIdFromRequestHeaders(), id);

            return NoContent();
        }

        // GET: api/User/GetQuantityOfUsers
        [HttpGet("GetQuantityOfUsers")]
        public async Task<ActionResult<int>> GetQuantityOfUsers()
        {
            LoggerWrapper.LogInformation("Get quantity of user models", GetType().Name, nameof(GetQuantityOfUsers), null);

            return Ok(await UserService.GetQuantityOfUsersAsync());
        }

        // POST: api/User/UsernameExists/John
        [HttpPost("UsernameExists/{username}")]
        [Authorize(Roles = Role.Service)]
        public async Task<ActionResult<bool>> Exists(string username)
        {
            LoggerWrapper.LogInformation($"User Model exists with name: {username}", GetType().Name, nameof(Exists), null);

            return Ok(await UserService.UsernameExistsAsync(username));
        }

        // POST: api/User/CredentialsValid
        [HttpPost("CredentialsValid")]
        [Authorize(Roles = Role.Service)]
        public async Task<ActionResult<bool>> AreCredentialsValid(UserModel model)
        {
            LoggerWrapper.LogInformation($"Credentials are valid: {model.Username}", GetType().Name, nameof(AreCredentialsValid), null);

            return Ok(await UserService.AreCredentialsValidAsync(model));
        }

        // GET: api/User/GetUserIdFromUsername/John
        [HttpGet("GetUserIdFromUsername/{username}")]
        [Authorize(Roles = Role.Service)]
        public async Task<ActionResult<UserModel>> GetUserIdFromUsername(string username)
        {
            LoggerWrapper.LogInformation($"Get user id from username: {username}", GetType().Name, nameof(GetUserIdFromUsername), null);

            return Ok(await UserService.GetUserIdFromUsername(username));
        }

        // GET: api/User/GetUserProfile/5
        [HttpGet("GetUserProfile")]
        [Authorize(Roles = Role.User + "," + Role.Administrator)]
        public async Task<ActionResult<UserModel>> GetUserProfile()
        {
            var userId = GetUserIdFromRequestHeaders();

            LoggerWrapper.LogInformation($"Get User Model By Id: {userId}", GetType().Name, nameof(GetUserProfile), null);

            return Ok(await UserService.GetByIdAsync(GetUserIdFromRequestHeaders(), null));
        }

        private string GetUserIdFromRequestHeaders()
        {
            var userId = TokenService.GetUserIdFromAuthorizationToken(HttpContext.Request.Headers["Authorization"]);

            LoggerWrapper.LogInformation("User Id from headers was: " + userId, this.GetType().Name, nameof(GetUserIdFromRequestHeaders), null);

            return userId;
        }
    }
}