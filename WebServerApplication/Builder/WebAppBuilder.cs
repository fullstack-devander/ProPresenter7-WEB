using ProPresenter7WEB.Service;
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
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddControllers()
                .AddApplicationPart(typeof(Program).Assembly)
                .AddControllersAsServices();

            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins("http://localhost:50890")));

            builder.Services.AddScoped<IPresentationService, PresentationService>();

            return new WebAppBuilder(builder);
        }

        public IWebAppBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            ArgumentNullException.ThrowIfNull(configureServices);

            configureServices(_webApplicationBuilder.Services);

            return this;
        }

        public WebApplication Build()
        {
            var app = _webApplicationBuilder.Build();

            app.UseStaticFiles();

            app.UseCors();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            return app;
        }
    }
}
