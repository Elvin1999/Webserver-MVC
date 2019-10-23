using System;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleApp1.Server
{
    public class AuthenticationMiddleware : IMiddleware
    {
        private HttpDelegate next;

        public AuthenticationMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context)
        {
            var url = context.Request.Url.LocalPath;

            if (url == "/auth")
            {
                var cookie = new Cookie("token", "hakuna");
                cookie.Expires = DateTime.Now.AddDays(1);
                context.Response.SetCookie(cookie);
            }

            if (url == "/logout")
            {
                if (context.Request.Cookies["token"] != null)
                {
                    var cookie = new Cookie("token", "hakuna");
                    cookie.Expires = DateTime.Now.AddDays(-1); // it is for force to delete cookie on client side
                    context.Response.SetCookie(cookie);
                }
            }

            if (context.Request.Cookies["token"] != null && context.Request.Cookies["token"].Value == "hakuna")
            {
                await next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 403;
                context.Response.StatusDescription = "Authorization needed (Forbidden)";
                context.Response.Close();
            }
        }
    }
}