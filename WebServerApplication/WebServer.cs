namespace ProPresenter7WEB.WebServerApplication
{
    public class WebServer : IWebServer
    {
        public void StartServer(string[]? args = null)
        {
            var builder = WebApplication.CreateBuilder(args: args);
            var app = builder.Build();

            app.UseStaticFiles();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        public void StopServer()
        {
            throw new NotImplementedException();
        }
    }
}
