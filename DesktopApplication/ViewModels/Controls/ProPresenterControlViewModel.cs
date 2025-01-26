using ProPresenter7WEB.DesktopApplication.Helpers;
using ProPresenter7WEB.DesktopApplication.Properties;
using ProPresenter7WEB.Service;
using System;
using System.Threading.Tasks;

namespace ProPresenter7WEB.DesktopApplication.ViewModels.Controls
{
    public class ProPresenterControlViewModel : ViewModelBase
    {
        private readonly IProPresenterService _proPresenterService;

        private bool _isConnected;
        private string _connectButtonText = ProPresenterControlResoures.ConnectButtonText;

        public ProPresenterControlViewModel(IProPresenterService proPresenterService)
        {
            _proPresenterService = proPresenterService;
        }

        public string IpAddressLabelText => ProPresenterControlResoures.IpAddressLabelText;
        public string PortLabelText => ProPresenterControlResoures.PortLabelText;

        public string? IpAddress { get; set; }

        public string? Port { get; set; }

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
                    await MessageBoxHelper
                        .GetFailedConnectionMessageBox(ProPresenterControlResoures.IpAddressIsEmptyFailMessage)
                        .ShowAsync();
                    return;
                }

                if (string.IsNullOrEmpty(Port))
                {
                    await MessageBoxHelper
                        .GetFailedConnectionMessageBox(ProPresenterControlResoures.PortIsEmptyFailMessage)
                        .ShowAsync();
                    return;
                }

                _proPresenterService.SetProPresenterConnection(IpAddress, int.Parse(Port));
                var response = await _proPresenterService.GetProPresenterInfoAsync();

                IsConnected = true;
                ConnectButtonText = ProPresenterControlResoures.DisconnectButtonText;
            }
            catch (Exception ex)
            {
                await MessageBoxHelper
                    .GetFailedConnectionMessageBox(ex.Message)
                    .ShowAsync();
            }
        }

        private void Disconnect()
        {
            IsConnected = false;
            ConnectButtonText = ProPresenterControlResoures.ConnectButtonText;
        }
    }
}
