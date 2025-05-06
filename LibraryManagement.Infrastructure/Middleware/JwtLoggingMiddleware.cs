using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Security.Claims;

namespace LibraryManagement.Infrastructure.Middleware
{
    public class JwtLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtLoggingMiddleware> _logger;

        public JwtLoggingMiddleware(RequestDelegate next, ILogger<JwtLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log thông tin về request
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst("sub")?.Value;
                var username = context.User.FindFirst(ClaimTypes.Name)?.Value;
                var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

                _logger.LogInformation(
                    "Authenticated request - UserId: {UserId}, Username: {Username}, Role: {Role}, Path: {Path}, Method: {Method}",
                    userId, username, role, context.Request.Path, context.Request.Method
                );
            }

            await _next(context);
        }
    }
}