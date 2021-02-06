using Microsoft.Extensions.DependencyInjection;
using Sartain_Studios_Common.Interfaces.Token;
using Services;

namespace Api.Controllers.Base
{
    public class BaseController : BaseWithLoggingController
    {
        private IUserService _userService;
        private IToken _tokenService;

        protected IUserService UserService => _userService ??= HttpContext?.RequestServices.GetService<IUserService>();
        protected IToken TokenService => _tokenService ??= HttpContext?.RequestServices.GetService<IToken>();
    }
}