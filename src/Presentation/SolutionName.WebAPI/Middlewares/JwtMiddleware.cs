using SolutionName.Application.Abstractions.Services;

namespace SolutionName.WebAPI.Middlewares
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly IJwtTokenService _jwtHandler;

        public JwtMiddleware(IJwtTokenService jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if ((token != null && !token.Equals("null")) || !string.IsNullOrEmpty(token) ||
                !string.IsNullOrWhiteSpace(token))
            {
                var validateResult = _jwtHandler.ValidateToken(token);
                if (validateResult.UserId!=null)
                    // attach user to context on successful jwt validation
                    context.Items["UserId"] = validateResult.UserId;
            }

            await next(context);
        }

       
    }
}
