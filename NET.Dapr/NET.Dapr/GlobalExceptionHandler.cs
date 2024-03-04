using Microsoft.AspNetCore.Diagnostics;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models.ApiModels;

namespace NET.Dapr
{
    public class GlobalExceptionHandler(ILoggerFactory loggerFactory) : IExceptionHandler
    {
        readonly ILogger logger = loggerFactory.CreateLogger<GlobalExceptionHandler>();
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError($"Exception occured {exception.Message}");
            httpContext.Response.StatusCode = 500;
            var response = new ApiResultModel<string>()
            {
                Success = false,
                Errors = [exception.Message, exception.InnerException?.Message],
                StackTraces = [exception.StackTrace, exception.InnerException?.StackTrace]
            };
            await httpContext.Response
            .WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
