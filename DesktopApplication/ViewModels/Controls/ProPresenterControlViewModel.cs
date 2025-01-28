using Microsoft.Extensions.Logging;
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
        private readonly IProPresenterInfoService _proPresenterInfoService;
        private readonly ILogger _logger;

        private bool _isConnected;
        private string _connectButtonText = ProPresenterControlResoures.ConnectButtonText;

        public ProPresenterControlViewModel(
            ILogger<ProPresenterControlViewModel> logger,
            IProPresenterService proPresenterService,
            IProPresenterInfoService proPresenterInfoService)
        {
            _logger = logger;
            _proPresenterService = proPresenterService;
            _proPresenterInfoService = proPresenterInfoService;

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

                _proPresenterService.SetApiAddress(
                    ProPresenterConnectModel.IpAddress, ProPresenterConnectModel.Port.Value);
                var proPresenterInfo = await _proPresenterInfoService.GetProPresenterInfoAsync();

                if (proPresenterInfo != null)
                {
                    IsConnected = true;
                    ConnectButtonText = ProPresenterControlResoures.DisconnectButtonText;

                    _logger.LogInformation(
                        $"Connection to ProPresenter {_proPresenterService.ApiAddress} established.");
                    _logger.LogInformation(
                        $"ProPresenter {proPresenterInfo.ApiVersion} is running on {proPresenterInfo.Platform}.");

                    ModelCacheHelper.SaveModelState(ProPresenterConnectModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Cannot establish connection: {ex.Message}");
                await MessageBoxHelper
                    .GetFailedConnectionMessageBox(ex.Message)
                    .ShowAsync();
            }
        }

        private void Disconnect()
        {
            IsConnected = false;

            _logger.LogInformation("Disconnected from ProPresenter.");
            ConnectButtonText = ProPresenterControlResoures.ConnectButtonText;
        }
    }
}
