namespace ProPresenter7WEB.WebServerApplication.Builder
{
    public interface IWebAppBuilder
    {
        IWebAppBuilder ConfigureServices(Action<IServiceCollection> services);

        WebApplication Build();
    }
}
