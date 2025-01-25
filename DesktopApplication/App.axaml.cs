using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
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

        public override void OnFrameworkInitializationCompleted()
        {
            var webApp = WebAppBuilder.CreateBuilder(Array.Empty<string>())
                .ConfigureServices(Startup.ConfigureServices)
                .Build();

            Startup.Configure(webApp, webApp.Environment);

            Services = webApp.Services;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                var mainWindow = Services.GetRequiredService<MainWindow>();
                mainWindow.WebApplication = webApp;
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