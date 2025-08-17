using System.Text.Json;

namespace IdentityAndAccess.API.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;
    private readonly IWebHostEnvironment _env;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var error = new Result(
                message: ex.Message,
                source: context.GetEndpoint()?.DisplayName ?? "",
                stackTrace: _env.IsDevelopment() ? ex.StackTrace : ""
            );

            var json = JsonSerializer.Serialize(error, JsonSerializerOptions);
            await context.Response.WriteAsync(json);
        }
    }
}