using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using ViralMusicAPI.Exceptions;
using BusinessObjects.DataTranferObjects;
using BusinessObjects.Exceptions;

namespace ViralMusicAPI.Handler
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponseDTO();
            switch (exception)
            {
                case NotFoundException ex:
                    errorResponse = new ErrorResponseDTO
                    (
                        "Resource Not Found",
                        (int)HttpStatusCode.NotFound,
                        ex.Message
                    );
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case BadRequestException ex:
                    errorResponse = new ErrorResponseDTO
                    (
                        "Bad Request",
                        (int)HttpStatusCode.BadRequest,
                        ex.Message
                    );
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedException ex:
                    errorResponse = new ErrorResponseDTO
                    (
                        "Unauthorized",
                        (int)HttpStatusCode.Unauthorized,
                        ex.Message
                    );
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case ForbiddenException ex:
                    errorResponse = new ErrorResponseDTO
                    (
                        "Forbidden",
                        (int)HttpStatusCode.Forbidden,
                        ex.Message
                    );
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                case Exception ex:
                    errorResponse = new ErrorResponseDTO
                    (
                        "Internal server error!",
                        (int)HttpStatusCode.InternalServerError,
                        ex.Message
                    );
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            _logger.LogError(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
