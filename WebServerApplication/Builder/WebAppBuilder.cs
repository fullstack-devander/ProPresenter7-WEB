using AspNetWebApplicationBuilder = Microsoft.AspNetCore.Builder.WebApplicationBuilder;

namespace ProPresenter7WEB.WebServerApplication.Builder
{
    public class WebAppBuilder : IWebAppBuilder
    {
        private readonly AspNetWebApplicationBuilder _webApplicationBuilder;

        private WebAppBuilder(AspNetWebApplicationBuilder builder)
        {
            _webApplicationBuilder = builder;
        }

        public static IWebAppBuilder CreateBuilder(string[] args)
        {
            return new WebAppBuilder(WebApplication.CreateBuilder(args));
        }

        public IWebAppBuilder ConfigureWebApplicationBuilder(Action<AspNetWebApplicationBuilder> configureWebApplicationBuilder)
        {
            ArgumentNullException.ThrowIfNull(configureWebApplicationBuilder);

            configureWebApplicationBuilder(_webApplicationBuilder);

            return this;
        }

        public IWebAppBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            ArgumentNullException.ThrowIfNull(configureServices);

            configureServices(_webApplicationBuilder.Services);

            return this;
        }

        public WebApplication Build()
        {
            return _webApplicationBuilder.Build();
        }
    }
}
