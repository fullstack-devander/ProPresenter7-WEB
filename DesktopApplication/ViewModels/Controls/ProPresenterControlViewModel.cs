using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ProPresenter7WEB.Service;
using System;
using System.Threading.Tasks;

namespace ProPresenter7WEB.DesktopApplication.ViewModels.Controls
{
    public class ProPresenterControlViewModel : ViewModelBase
    {
        private readonly IProPresenterService _proPresenterService;

        private bool _isConnected;
        private string _connectButtonText = "Connect";

        public ProPresenterControlViewModel(IProPresenterService proPresenterService)
        {
            _proPresenterService = proPresenterService;
        }

        public string ConnectButtonText
        {
            get => _connectButtonText;
            set
            {
                if (_connectButtonText != value)
                {
                    _connectButtonText = value;
                    OnPropertyChanged(nameof(ConnectButtonText));
                }
            }
        }

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }

        public string? IpAddress { get; set; }

        public string? Port { get; set; }

        public async void ClickConnectButton()
        {
            if (_isConnected)
            {
                Disconnect();
            }
            else
            {
                await ConnectAsync();
            }
        }

        private async Task ConnectAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(IpAddress))
                {
                    await ShowFailedConnectionMessageBoxAsync("'Ip Address' cannot be empty.");
                    return;
                }

                if (string.IsNullOrEmpty(Port))
                {
                    await ShowFailedConnectionMessageBoxAsync("'Port' cannot be empty.");
                    return;
                }

                _proPresenterService.SetProPresenterConnection(IpAddress, int.Parse(Port));
                var response = await _proPresenterService.GetProPresenterInfoAsync();

                IsConnected = true;
                ConnectButtonText = "Disconnect";
            }
            catch (Exception ex)
            {
                await ShowFailedConnectionMessageBoxAsync(ex.Message);
            }
        }

        private void Disconnect()
        {
            IsConnected = false;
            ConnectButtonText = "Connect";
        }

        private async static Task ShowFailedConnectionMessageBoxAsync(string message)
        {
            await MessageBoxManager
                .GetMessageBoxStandard($"Failed Connection", message, ButtonEnum.Ok)
                .ShowAsync();
        }
    }
}
