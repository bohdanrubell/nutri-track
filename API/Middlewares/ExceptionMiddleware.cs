using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NutriTrack.Exceptions;

namespace NutriTrack.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
           
            
            context.Response.StatusCode = ex switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                UserIsNotAuthorizedException => StatusCodes.Status401Unauthorized,
                UserDoesNotExistException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            object responseObject;
    
            // Special handling for ValidationException with multiple errors
            if (ex is ValidationException validationEx && validationEx.HasValidationErrors)
            {
                responseObject = new 
                {
                    Title = ex.Message,
                    Status = context.Response.StatusCode,
                    /*Detail = _env.IsDevelopment() ? ex.StackTrace : null,*/
                    Type = ex.GetType().Name,
                    Errors = validationEx.ValidationErrors
                };
            }
            else 
            {
                responseObject = new ProblemDetails
                {
                    Status = context.Response.StatusCode,
                    /*Detail = _env.IsDevelopment() ? ex.StackTrace : null,*/
                    Title = ex.Message,
                    /*Type = ex.GetType().Name*/
                };
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(responseObject, options);

            await context.Response.WriteAsync(json);
        }
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseException(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}