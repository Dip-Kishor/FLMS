using FLMS.Services.TokenValidation;

namespace FLMS.Web
{
    public class TokenMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public TokenMiddleWare(RequestDelegate next, ITokenBlacklistService tokenBlacklistService)
        {
            _next = next;
            _tokenBlacklistService = tokenBlacklistService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["accessToken"];
            if (!string.IsNullOrEmpty(token) && await _tokenBlacklistService.IsTokenRevoked(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token has been revoked.");
                return;
            }

            await _next(context);
        }
    }

}
