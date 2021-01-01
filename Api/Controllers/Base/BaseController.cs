using Microsoft.Extensions.DependencyInjection;
using Services;

namespace Api.Controllers.Base
{
    public class BaseController : BaseWithLoggingController
    {
        private IUserService _userService;

        protected IUserService UserService => _userService ??= HttpContext?.RequestServices.GetService<IUserService>();
    }
}