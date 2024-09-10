using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using app.DAL.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using app.BLL.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace app.BLL
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger<User> logger;

        public ExceptionHandler(RequestDelegate next, ILogger<User> logger)
        {
            this.next = next;
            this.logger = logger;

        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await handleExceptionAsync(httpContext, ex);
            }

        }
        public async Task handleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode;
            string message;
            var exception = ex.GetType();
            if (exception == typeof(NotFoundException))
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = ex.Message;
            }
            else if (exception == typeof(BadRequestException))
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = ex.Message;
            }
            else if (exception == typeof(UnauthorizedAccessException))
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = ex.Message;
            }
            else if (exception == typeof(NotImplementedException))
            {
                statusCode = (int)HttpStatusCode.NotImplemented;
                message = ex.Message;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = ex.Message;
            }
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var exceptionResult = JsonSerializer.Serialize(new
            {
                Message = message,
                StatusCode = statusCode

            });
            logger.LogError(exceptionResult.ToString());

            await context.Response.WriteAsync(exceptionResult);



        }

    }
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            return app;
        }
    }
}

