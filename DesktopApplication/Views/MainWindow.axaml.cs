using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.AspNetCore.Builder;
using ProPresenter7WEB.DesktopApplication.ViewModels;
using ProPresenter7WEB.Service;
using System.Threading.Tasks;

namespace ProPresenter7WEB.DesktopApplication.Views
{
    public partial class MainWindow : Window
    {
        private readonly ISharedService _sharedService;
        private bool _isServerRunning;

        public MainWindow(
            MainWindowViewModel mainWindowViewModel,
            ISharedService sharedService)
        {
            _sharedService = sharedService;
            DataContext = mainWindowViewModel;

            InitializeComponent();
        }

        public required WebApplication WebApplication { get; set; }

        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            if (_isServerRunning)
            {
                await StopServer();
            }

            base.OnClosing(e);
        }

        private async void OnStartServerClick(object sender, RoutedEventArgs args)
        {
            if (!_isServerRunning)
            {
                await StartServer();
            }
            else
            {
                await StopServer();
            }
        }

        private async Task StartServer()
        {
            _sharedService.Data = "Hello";

            await WebApplication.StartAsync();

            _isServerRunning = true;

            StartServerButton.Content = "Stop Server";
        }

        private async Task StopServer()
        {
            _sharedService.Data = "Goodbye";

            if (WebApplication != null)
            {
                await WebApplication.StopAsync();
                await WebApplication.DisposeAsync();
                _isServerRunning = false;

                StartServerButton.Content = "Start Server";
            }
        }
    }
}