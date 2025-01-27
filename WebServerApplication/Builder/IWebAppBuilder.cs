namespace ProPresenter7WEB.WebServerApplication.Builder
{
    public interface IWebAppBuilder
    {
        IWebAppBuilder ConfigureServices(Action<IServiceCollection> services);

        IWebAppBuilder ConfigureWebApplicationBuilder(Action<WebApplicationBuilder> configureWebApplicationBuilder);

        WebApplication Build();
    }
}
