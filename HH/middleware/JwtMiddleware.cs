using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);

            var sidClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "sid");
            if (sidClaim != null)
            {
                string sid = sidClaim.Value;
                // 在这里将 sid 存储到 HttpContext 中，以便后续请求使用
                context.Items["Sid"] = sid;
            }
        }

        await _next(context);
    }
}