using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sartain_Studios_Common.Logging;

namespace Api.Controllers.Base
{
    public class BaseWithLoggingController : ControllerBase
    {
        private ILoggerWrapper _loggerWrapper;

        protected ILoggerWrapper LoggerWrapper =>
            _loggerWrapper ??= HttpContext?.RequestServices.GetService<ILoggerWrapper>();
    }
}