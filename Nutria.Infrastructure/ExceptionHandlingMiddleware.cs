using Microsoft.AspNetCore.Http;

namespace Nutria.Infrastructure;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex) // Captura la excepción específica que lanzamos
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new { message = ex.Message };
            await context.Response.WriteAsJsonAsync(response);
            Console.WriteLine(ex.Message);
        }
        catch (Exception) // Captura cualquier otro error inesperado (Error 500)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new { message = "Ocurrió un error interno en el servidor." };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}