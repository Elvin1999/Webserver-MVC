using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Server
{
   public class HttpAttribute:Attribute
    {
        public string Method { get; set; }

        public HttpAttribute(string method)
        {
            if(method.ToLower()!="get" && method.ToLower() != "post")
            {
                throw new ArgumentException("Method must be GET or POST");
            }
            Method = method;
        }
    }
}
