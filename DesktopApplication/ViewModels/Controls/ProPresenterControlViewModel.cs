using ProPresenter7WEB.DesktopApplication.Helpers;
using ProPresenter7WEB.DesktopApplication.Models;
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
            ProPresenterConnectModel = ModelCacheHelper
                .ReadModelState<ProPresenterConnectModel>() ?? new ProPresenterConnectModel();
        }

        public string IpAddressLabelText => ProPresenterControlResoures.IpAddressLabelText;
        public string PortLabelText => ProPresenterControlResoures.PortLabelText;

        public ProPresenterConnectModel ProPresenterConnectModel { get; set; }

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
                if (string.IsNullOrEmpty(ProPresenterConnectModel.IpAddress))
                {
                    await MessageBoxHelper
                        .GetFailedConnectionMessageBox(ProPresenterControlResoures.IpAddressIsEmptyFailMessage)
                        .ShowAsync();
                    return;
                }

                if (ProPresenterConnectModel.Port == null)
                {
                    await MessageBoxHelper
                        .GetFailedConnectionMessageBox(ProPresenterControlResoures.PortIsEmptyFailMessage)
                        .ShowAsync();
                    return;
                }

                _proPresenterService.SetProPresenterConnection(
                    ProPresenterConnectModel.IpAddress, ProPresenterConnectModel.Port.Value);
                var response = await _proPresenterService.GetProPresenterInfoAsync();

                if (response != null)
                {
                    IsConnected = true;
                    ConnectButtonText = ProPresenterControlResoures.DisconnectButtonText;
                    ModelCacheHelper.SaveModelState(ProPresenterConnectModel);
                }
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
