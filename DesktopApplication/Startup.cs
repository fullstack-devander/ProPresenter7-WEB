using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProPresenter7WEB.DesktopApplication.Factories;
using ProPresenter7WEB.DesktopApplication.ViewModels;
using ProPresenter7WEB.DesktopApplication.ViewModels.Controls;
using ProPresenter7WEB.DesktopApplication.Views;
using ProPresenter7WEB.Service;
using System.Reflection;

namespace ProPresenter7WEB.DesktopApplication
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(Assembly.Load("ProPresenter7WEB.WebServerApplication"))
                .AddControllersAsServices();

            services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins("http://localhost:50890")));

            services.AddHttpClient();
            services.AddLogging();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<ProPresenterControlViewModel>();
            services.AddSingleton<IAutoMapperFactory, AutoMapperFactory>();
            services.AddSingleton(services => services.GetRequiredService<IAutoMapperFactory>().Create());

            services.AddSingleton<IProPresenterService, ProPresenterService>();
            services.AddSingleton<IPresentationStorageService, PresentationStorageService>();
            services.AddScoped<IProPresenterInfoService, ProPresenterInfoService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
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
