using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Server
{
    public class MiddlewareBuilder
    {
        private Stack<Type> types = new Stack<Type>();

        public MiddlewareBuilder Use<T>()
        {
            int count = typeof(T).GetConstructors().Where(x =>
                x.GetParameters().Count() == 1 && 
                x.GetParameters()[0].ParameterType == typeof(HttpDelegate))
                .Count();
            if (count == 0)
            {
                throw new Exception("Middleware need have constructor with HttpDelegate param");
            }

            types.Push(typeof(T));

            return this;
        }

        public HttpDelegate Build()
        {
            HttpDelegate handler = async (ctx) =>
            {
                ctx.Response.Close();
            };

            while (types.Count > 0)
            {
                handler = (Activator.CreateInstance(types.Pop(), handler) as IMiddleware).InvokeAsync;
            }

            return handler;
        }
    }
}