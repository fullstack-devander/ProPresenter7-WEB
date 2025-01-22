using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;
using WebAppProgram = ProPresenter7WEB.WebServerApplication.Program;

namespace ProPresenter7WEB.DesktopApplication.Views
{
    public partial class MainWindow : Window
    {
        private WebApplication? _webApplication;
        private bool _isServerRunning;

        public MainWindow()
        {
            InitializeComponent();
        }

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
            _webApplication = new WebAppProgram().CreateApplication(Array.Empty<string>());
            await _webApplication.StartAsync();

            var config  = _webApplication.Configuration;

            _isServerRunning = true;

            StartServerButton.Content = "Stop Server";
        }

        private async Task StopServer()
        {
            if (_webApplication != null)
            {
                await _webApplication.StopAsync();
                await _webApplication.DisposeAsync();
                _webApplication = null;
                _isServerRunning = false;

                StartServerButton.Content = "Start Server";
            }
        }
    }
}