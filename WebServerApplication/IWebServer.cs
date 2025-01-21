namespace ProPresenter7WEB.WebServerApplication
{
    public interface IWebServer
    {
        void StartServer(string[]? args);
        void StopServer();
    }
}
