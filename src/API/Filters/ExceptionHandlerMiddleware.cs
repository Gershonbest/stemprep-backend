using Serilog;
using System.Text.Json;
using Application.Common.Exceptions;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Filters
{
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {

        try
        {
            await _next(httpContext);
        }
        catch (Exception error)
        {
            Result responseModel = null;
            var response = httpContext.Response;
            response.ContentType = "application/json";
            string msg = "An error occured, please try again later!";
            switch (error)
            {
                case UnauthorizedAccessException _:
                    msg = "You are not authorized!";
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;

                case ValidationException validationException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    msg = $"{validationException.Message ?? validationException.InnerException.Message}. Error : {validationException.GetErrors()}";
                    break;

                case NotFoundException _:
                    msg = "The specified resource was not found.";
                    response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case FluentValidation.ValidationException validationException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    List<string> errorMessages = validationException.Errors
                                                           .Select(e => $"{e.ErrorMessage}")
                                                           .ToList();
                    msg = $"Errors: {string.Join("; ", errorMessages)}";
                    break;
                case DbUpdateException ex:
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        msg = "Update DB Error" + ex.Message;
                        break;

                    default:
                    Log.Error(error.ToString());
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }
            responseModel = new Result { Message = msg, Succeeded = false };
            var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await response.WriteAsync(result);
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}

}


