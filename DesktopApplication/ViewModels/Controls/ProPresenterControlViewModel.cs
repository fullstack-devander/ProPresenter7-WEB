using ProPresenter7WEB.Service;

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
            _proPresenterService.SetProPresenterConnection(IpAddress, int.Parse(Port));
            var response = await _proPresenterService.GetProPresenterInfoAsync();
        }
    }
}
