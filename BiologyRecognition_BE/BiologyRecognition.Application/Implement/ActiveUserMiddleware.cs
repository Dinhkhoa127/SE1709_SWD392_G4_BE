using BiologyRecognition.Application.Interface;
using BiologyRecognition.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Implement
{
    public class ActiveUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserAccountRepository _repository;
        private static readonly string[] _excludedPaths = new[]
    {
    "/api/authentication/logout",
    };

        public ActiveUserMiddleware(RequestDelegate next)
        {
            _next = next;
            _repository = new UserAccountRepository();
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var requestPath = context.Request.Path.Value?.ToLower();

            // Nếu route nằm trong danh sách loại trừ → bỏ qua kiểm tra IsActive
            if (_excludedPaths.Contains(requestPath))
            {
                await _next(context);
                return;
            }
            if (context.User.Identity.IsAuthenticated)
            {
                var userIdClaim = context.User.FindFirst("Id")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    var user = await _repository.GetByIdAsync(userId);

                    if (user != null && !user.IsActive)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Your account has been banned.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }

}
