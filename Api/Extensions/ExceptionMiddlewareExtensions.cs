using System;
using System.Net;
using Api.Models;
using Consumables.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Sartain_Studios_Common.Logging;
using Services.Exceptions;

namespace Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerWrapper loggerWrapper)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var errorDetailsModel = new ErrorDetailsModel
                    {
                        StatusCode = context.Response.StatusCode
                    };

                    try
                    {
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                        var nameOfThrownException = contextFeature.Error.GetType().Name;

                        loggerWrapper.LogError("Name of exception thrown: " + nameOfThrownException,
                            nameof(ConfigureExceptionHandler), nameof(ConfigureExceptionHandler), null);

                        switch (nameOfThrownException)
                        {
                            case nameof(UserDoesNotExistException):
                                errorDetailsModel.StatusCode = (int) HttpStatusCode.NotAcceptable;
                                break;
                            case nameof(AlreadyInUseException):
                                errorDetailsModel.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                                break;
                            case nameof(PasswordContainsUsernameException):
                                errorDetailsModel.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                                break;
                            default:
                                errorDetailsModel.StatusCode = (int) HttpStatusCode.InternalServerError;
                                break;
                        }

                        errorDetailsModel.Message = contextFeature.Error.Message;
                        
                        loggerWrapper.LogError(contextFeature.Error.Message, nameof(ConfigureExceptionHandler),
                            nameof(ConfigureExceptionHandler), null);
                        
                        if (contextFeature.Error.InnerException != null)
                            loggerWrapper.LogError(contextFeature.Error.InnerException.Message,
                                nameof(ConfigureExceptionHandler), nameof(ConfigureExceptionHandler), null);
                    }
                    catch (Exception)
                    {
                        loggerWrapper.LogError("Unable to determine ContextFeature", nameof(ConfigureExceptionHandler),
                            nameof(ConfigureExceptionHandler), null);

                        errorDetailsModel.StatusCode = (int) HttpStatusCode.InternalServerError;
                        errorDetailsModel.Message = "Unknown error occurred";
                    }

                    loggerWrapper.LogError("Status code returned to client: " + errorDetailsModel.StatusCode,
                        nameof(ConfigureExceptionHandler), nameof(ConfigureExceptionHandler), null);
                    loggerWrapper.LogError("Message returned to client: " + errorDetailsModel.Message,
                        nameof(ConfigureExceptionHandler), nameof(ConfigureExceptionHandler), null);
                    await context.Response.WriteAsync(errorDetailsModel.ToString());
                });
            });
        }
    }
}