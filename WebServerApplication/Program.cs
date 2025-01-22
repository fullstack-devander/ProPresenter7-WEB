using ProPresenter7WEB.Service;

namespace ProPresenter7WEB.WebServerApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            var webApp = new Program().CreateApplication(args);
            webApp.Run();
        }

        public WebApplication CreateApplication(string[] args)
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

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseCors();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            return app;
        }
    }
}