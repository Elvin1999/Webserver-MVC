using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ConsoleApp1.Server
{
    public class MvcMiddleware : IMiddleware
    {
        private HttpDelegate next;

        public MvcMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context)
        {
            var response = context.Response;
            var writer = new StreamWriter(response.OutputStream);
            try
            {
                var resp = FindControllerAction(context.Request);

                if (resp != null)
                {
                    response.StatusCode = 200;
                    response.ContentType = "text/html";
                    writer.Write(resp);
                }
                else
                {
                    await next(context);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ContentType = "text/plain";
                writer.Write(ex.Message);
            }
            finally
            {
                writer.Close();
            }
        }

        public string FindControllerAction(HttpListenerRequest request)
        {
            string[] urlParts = request.RawUrl.Split(new[] { '/', '?' }, StringSplitOptions.RemoveEmptyEntries);
            if (urlParts.Length < 2)
                return null;

            string controller = urlParts[0];
            string action = urlParts[1];

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Type controllerType = currentAssembly.GetType($"ConsoleApp1.Controllers.{controller}Controller", false, true);
            if (controllerType is null)
                return null;

            MethodInfo actionMethod = controllerType.GetMethod(action, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (actionMethod is null)
                return null;
            HttpAttribute attribute = actionMethod.GetCustomAttribute<HttpAttribute>();
            if (attribute == null)
            {
                attribute = new HttpAttribute("get");
            }
            if (attribute.Method.ToLower() != request.HttpMethod.ToLower())
            {
                return null;
            }
            var paramsToMethod = new List<object>();
            NameValueCollection coll = null;

            if (request.HttpMethod == "GET")
            {
                if (urlParts.Length == 2 && actionMethod.GetParameters().Length != 0)
                    return null;

                if (urlParts.Length > 2)
                {
                    coll = System.Web.HttpUtility.ParseQueryString(urlParts[2]);
                }
            }
            else if (request.HttpMethod == "POST")
            {
                string body;
                using (StreamReader reader = new StreamReader(request.InputStream))
                {
                    body = reader.ReadToEnd();
                }
                coll = System.Web.HttpUtility.ParseQueryString(body);
            }
            else
            {
                return null;
            }

            ParameterInfo[] parameters = actionMethod.GetParameters();
            foreach (var pi in parameters)
            {
                paramsToMethod.Add(Convert.ChangeType(coll[pi.Name], pi.ParameterType));
            }
            if (paramsToMethod.Count != actionMethod.GetParameters().Length)
                return null;
            string resp = actionMethod.Invoke(Activator.CreateInstance(controllerType), paramsToMethod.ToArray()) as string;

            return resp;
        }
    }
}