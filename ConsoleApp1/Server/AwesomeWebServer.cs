using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleApp1.Server
{
    public class AwesomeWebServer
    {
        private readonly string _domain;
        private readonly string _port;
        private readonly HttpListener _httpListener;
        private HttpDelegate firstMiddleware;
        public AwesomeWebServer(string domain, string port)
        {
            _domain = domain;
            _port = port;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add($"{domain}:{port}/");
        }

        public AwesomeWebServer Configure<T>() where T : IConfigurator, new()
        {
            IConfigurator configurator = new T();
            var builder = new MiddlewareBuilder();
            configurator.ConfigureMiddleware(builder);
            firstMiddleware = builder.Build();
            return this;
        }

        public void Run()
        {
            _httpListener.Start();
            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext();
                Task.Run(() => Process(context));
            }
        }

        public void Process(HttpListenerContext context)
        {
            try
            {
                firstMiddleware(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                using (var writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write($"Error on Awesome Web Server: {ex.Message}");
                }
            }
        }
    }
}