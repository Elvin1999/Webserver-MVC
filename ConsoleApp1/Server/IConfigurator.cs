namespace ConsoleApp1.Server
{
    public interface IConfigurator
    {
        void ConfigureMiddleware(MiddlewareBuilder mb);
    }

    public class Configurator : IConfigurator
    {
        public void ConfigureMiddleware(MiddlewareBuilder mb)
        {
            mb.Use<MvcMiddleware>();
        }
    }
}