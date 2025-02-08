using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using ProPresenter7WEB.Core;
using ProPresenter7WEB.DesktopApplication.Helpers;
using ProPresenter7WEB.DesktopApplication.Models;
using ProPresenter7WEB.DesktopApplication.Properties;
using ProPresenter7WEB.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProPresenter7WEB.DesktopApplication.ViewModels.Controls
{
    public class ProPresenterControlViewModel : ViewModelBase
    {
        private readonly IProPresenterService _proPresenterService;
        private readonly IProPresenterInfoService _proPresenterInfoService;
        private readonly IPlaylistService _playlistService;
        private readonly IPresentationStorageService _presentationStorageService;
        private readonly ILogger _logger;

        private bool _isConnected;
        private bool _isSelectedPresentationApplied;
        private string _connectButtonText = ProPresenterControlResoures.ConnectButtonText;
        private string _applyButtonText = ProPresenterControlResoures.ApplyButtonText;
        private ObservableCollection<Playlist>? _playlists;
        private Playlist? _selectedPlaylist;
        private ObservableCollection<PlaylistDetailsPresentation>? _presentations;
        private PlaylistDetailsPresentation? _selectedPresentation;

        public ProPresenterControlViewModel(
            ILogger<ProPresenterControlViewModel> logger,
            IProPresenterService proPresenterService,
            IProPresenterInfoService proPresenterInfoService,
            IPlaylistService playlistService,
            IPresentationStorageService presentationStorageService)
        {
            _logger = logger;
            _proPresenterService = proPresenterService;
            _proPresenterInfoService = proPresenterInfoService;
            _playlistService = playlistService;
            _presentationStorageService = presentationStorageService;

            ProPresenterConnectModel = ModelCacheHelper
                .ReadModelState<ProPresenterConnectModel>() ?? new ProPresenterConnectModel();
        }

        #region UITextProperties
        public string IpAddressLabelText => ProPresenterControlResoures.IpAddressLabelText;
        public string PortLabelText => ProPresenterControlResoures.PortLabelText;
        public string PlaylistLabelText => ProPresenterControlResoures.PlaylistLabelText;
        public string PresentationLabelText => ProPresenterControlResoures.PresentationLabelText;

        public string ConnectButtonText
        {
            get => _connectButtonText;
            set => SetProperty(ref _connectButtonText, value);
        }

        public string ApplyButtonText
        {
            get => _applyButtonText;
            set => SetProperty(ref _applyButtonText, value);
        }
        #endregion

        public ProPresenterConnectModel ProPresenterConnectModel { get; set; }

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public bool IsSelectedPresentationApplied
        {
            get => _isSelectedPresentationApplied;
            set => SetProperty(ref _isSelectedPresentationApplied, value);
        }

        public ObservableCollection<Playlist>? Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        public Playlist? SelectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                if (SetProperty(ref _selectedPlaylist, value))
                {
                    Dispatcher.UIThread.Post(async () => await InitPresentationsSourceAsync());
                }
            }
        }

        public ObservableCollection<PlaylistDetailsPresentation>? Presentations
        {
            get => _presentations;
            set => SetProperty(ref _presentations, value);
        }

        public PlaylistDetailsPresentation? SelectedPresentation
        {
            get => _selectedPresentation;
            set => SetProperty(ref _selectedPresentation, value);
        }

        public async void ClickConnectButton()
        {
            if (IsConnected)
            {
                Disconnect();
            }
            else
            {
                await ConnectAsync();
            }
        }

        public async void ClickApplyButton()
        {
            if (!IsSelectedPresentationApplied)
            {
                await ApplySelectedPresentation();
            }
            else
            {
                CancelSelectedPresentation();
            }
        }

        private async Task ApplySelectedPresentation()
        {
            if (SelectedPresentation == null)
            {
                _logger.LogWarning("Presentation is not selected when clicked \"Apply\" button.");

                await MessageBoxHelper
                    .GetValidationFailedMessageBox(ProPresenterControlResoures.PresentationIsEmptyFailMessage)
                    .ShowAsync();

                return;
            }

            _presentationStorageService.SetPresentationUuid(SelectedPresentation.Uuid);
            IsSelectedPresentationApplied = true;
            ApplyButtonText = ProPresenterControlResoures.UpdateButtonText;

            _logger.LogInformation("Presentation {0} is applied to use on Web UI.", SelectedPresentation.Uuid);
        }

        private void CancelSelectedPresentation()
        {
            var selectedPresentationUuid = _presentationStorageService.GetPresentationUuid();
            
            _presentationStorageService.RemovePresentationUuid();
            IsSelectedPresentationApplied = false;
            ApplyButtonText = ProPresenterControlResoures.ApplyButtonText;

            _logger.LogInformation("Presentation {0} is cancelled to use on Web UI.", selectedPresentationUuid);
        }

        private async Task InitPlaylistsSourceAsync()
        {
            try
            {
                var playlists = await _playlistService.GetPlaylistsAsync();
                Playlists = new ObservableCollection<Playlist>(playlists);
                SelectedPlaylist = Playlists.First();
                
                _logger.LogInformation("Initialized playlist list with {0} items. Selected playlist uuid: {1}.", 
                    playlists.Count(), SelectedPlaylist.Uuid);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Cannot get playlist list: {ex.Message}");
            }
        }

        private async Task InitPresentationsSourceAsync()
        {
            try
            {
                if (SelectedPlaylist == null)
                {
                    _logger.LogWarning("Playlist is not selected when presentations is initializing.");
                    return;
                }

                var playlistDetails = await _playlistService.GetPlayListDetailsAsync(SelectedPlaylist.Uuid);
                Presentations = new ObservableCollection<PlaylistDetailsPresentation>(playlistDetails.Presentations);
                SelectedPresentation = Presentations.First();

                _logger.LogInformation("Initialized presentation lists with {0} items. Selected presentation uuid: {1}.",
                    playlistDetails.Presentations.Count(), SelectedPresentation.Uuid);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Cannot get presentation list: {ex.Message}");
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

                    _logger.LogWarning("IP address is empty when connecting to ProPresenter.");

                    return;
                }

                if (ProPresenterConnectModel.Port == null)
                {
                    await MessageBoxHelper
                        .GetFailedConnectionMessageBox(ProPresenterControlResoures.PortIsEmptyFailMessage)
                        .ShowAsync();

                    _logger.LogWarning("Port is empty when connecting to ProPresenter.");

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

                    await InitPlaylistsSourceAsync();

                    ModelCacheHelper.SaveModelState(ProPresenterConnectModel);
                }
            }
            catch (Exception ex)
            {
                await MessageBoxHelper
                    .GetFailedConnectionMessageBox(ex.Message)
                    .ShowAsync();

                _logger.LogWarning($"Cannot establish connection: {ex.Message}");
            }
        }

        private void Disconnect()
        {
            IsConnected = false;
            SelectedPlaylist = null;
            SelectedPresentation = null;
            _presentationStorageService.RemovePresentationUuid();
            IsSelectedPresentationApplied = false;

            ApplyButtonText = ProPresenterControlResoures.ApplyButtonText;
            ConnectButtonText = ProPresenterControlResoures.ConnectButtonText;

            _logger.LogInformation("Disconnect ProPresenter server.");
        }
    }
}
