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

        public ProPresenterControlViewModel(IProPresenterService proPresenterService)
        {
            _proPresenterService = proPresenterService;
        }

        public string? IpAddress { get; set; }

        public string? Port { get; set; }

        public async void Connect()
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
            }
            catch (Exception ex)
            {
                await ShowFailedConnectionMessageBoxAsync(ex.Message);
            }
        }

        private async static Task ShowFailedConnectionMessageBoxAsync(string message)
        {
            await MessageBoxManager
                .GetMessageBoxStandard($"Failed Connection", message, ButtonEnum.Ok)
                .ShowAsync();
        }
    }
}
