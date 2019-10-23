using ConsoleApp1.Server;
using System;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var server = new AwesomeWebServer("http://127.0.0.1", "8084").Configure<Configurator>();
                Console.WriteLine("Server is running...");
                server.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server stopped");
                Console.WriteLine(ex.Message);
            }
        }
    }
}