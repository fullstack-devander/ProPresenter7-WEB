using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProPresenter7WEB.DesktopApplication.ViewModels;
using ProPresenter7WEB.DesktopApplication.ViewModels.Controls;
using ProPresenter7WEB.DesktopApplication.Views;
using ProPresenter7WEB.Service;

namespace ProPresenter7WEB.DesktopApplication
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(typeof(Program).Assembly)
                .AddControllersAsServices();

            services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins("http://localhost:50890")));

            services.AddHttpClient();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<ProPresenterControlViewModel>();

            services.AddSingleton<IProPresenterService, ProPresenterService>();
            services.AddSingleton<ISharedService, SharedService>();

            services.AddScoped<IPresentationService, PresentationService>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCors();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("/index.html");
            });
        }
    }
}
