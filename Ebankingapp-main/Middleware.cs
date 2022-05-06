using DbHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EbankingApp
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        private static IDataHelper _dbHelper;

        public Middleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext httpContext, IDataHelper dbHelper)
        {
            _dbHelper = dbHelper;
            var users = await _dbHelper.GetUsersAsync();
            await _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}
