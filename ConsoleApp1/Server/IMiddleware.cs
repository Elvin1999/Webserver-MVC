using System.Net;
using System.Threading.Tasks;

namespace ConsoleApp1.Server
{
    public interface IMiddleware
    {
        Task InvokeAsync(HttpListenerContext context);
    }

    public delegate Task HttpDelegate(HttpListenerContext context);
    public class Middleware1 : IMiddleware
    {
        private HttpDelegate next;
        public Middleware1(HttpDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpListenerContext context)
        {
            System.Console.WriteLine("Middleware 1 begin work " + context.Request.Url.AbsoluteUri);
            await next.Invoke(context);
            System.Console.WriteLine("Middleware 1 ends work");
        }
    }

    public class Middleware2 : IMiddleware
    {
        private HttpDelegate next;

        public Middleware2(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context)
        {
            System.Console.WriteLine("Middleware 2 begin work " + context.Request.Url.AbsoluteUri);
            await next.Invoke(context);
            System.Console.WriteLine("Middleware 2 ends work");
        }
    }

}