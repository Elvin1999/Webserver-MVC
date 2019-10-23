using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp1.Server
{
    public class MyMiddleware : IMiddleware
    {
        private HttpDelegate next;

        public MyMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context)
        {
            var books = new List<string>() { "A", "B", "C" };

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            StreamReader reader = new StreamReader(request.InputStream);
            StreamWriter writer = new StreamWriter(response.OutputStream);

            var url = request.Url.LocalPath;
            var method = request.HttpMethod;

            try
            {
                // /books
                if (url == "/books" && method == "GET")
                {
                    StringBuilder resp = new StringBuilder()
                        .Append("<html><body>")
                        .Append("<h3>All books:</h3>")
                        .Append("<ul>");

                    foreach (var book in books)
                    {
                        resp.Append($"<li>{book}</li>");
                    }

                    resp.Append("</ul>")
                        .Append("<br>")
                        .Append("<form action='/addbook' method='post'>")
                        .Append("<input type='text' name='title'>")
                        .Append("<input type='submit' value='Add book'>")
                        .Append("</form>")
                        .Append("</body></html>");

                    response.ContentType = "text/html";
                    response.StatusCode = 200;

                    writer.Write(resp);
                }
                else if (url == "/addbook" && method == "POST")
                {
                    var data = HttpUtility.ParseQueryString(reader.ReadToEnd());

                    books.Add(data["title"]);

                    StringBuilder resp = new StringBuilder()
                       .Append("<html><body>")
                       .Append("<h3>All books:</h3>")
                       .Append("<ul>");

                    foreach (var book in books)
                    {
                        resp.Append($"<li>{book}</li>");
                    }

                    resp.Append("</ul>")
                        .Append("<br>")
                        .Append("<form action='/addbook' method='post'>")
                        .Append("<input type='text' name='title'>")
                        .Append("<input type='submit' value='Add book'>")
                        .Append("</form>")
                        .Append("</body></html>");

                    response.ContentType = "text/html";
                    response.StatusCode = 200;

                    writer.Write(resp);
                }
                else
                {
                    await next.Invoke(context);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer.Close();
            }
        }
    }
}
