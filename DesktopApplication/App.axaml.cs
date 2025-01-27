using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using ProPresenter7WEB.DesktopApplication.Views;
using ProPresenter7WEB.WebServerApplication.Builder;
using System;
using System.Linq;

namespace ProPresenter7WEB.DesktopApplication
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static IServiceProvider? Services { get; private set; }

        public static WebApplication? WebApplication { get; private set; }

        public override void OnFrameworkInitializationCompleted()
        {
            WebApplication = WebAppBuilder.CreateBuilder(Array.Empty<string>())
                .ConfigureWebApplicationBuilder(builder =>
                {
                    builder.Logging.ClearProviders();
                    builder.Host.UseNLog();
                })
                .ConfigureServices(Startup.ConfigureServices)
                .Build();

            Startup.Configure(WebApplication, WebApplication.Environment);

            Services = WebApplication.Services;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                var mainWindow = Services.GetRequiredService<MainWindow>();
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}