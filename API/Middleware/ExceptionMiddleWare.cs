using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleWare(IHostEnvironment env, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, env);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment v)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = v.IsDevelopment()
            ? new ApiErrorResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
            : new ApiErrorResponse((int)HttpStatusCode.InternalServerError, "Internal Server Error", "Internal Server Error");

        var options = new JsonSerializerOptions
        { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var json = JsonSerializer.Serialize(response, options);
        return context.Response.WriteAsync(json, System.Text.Encoding.UTF8);
    }
}

