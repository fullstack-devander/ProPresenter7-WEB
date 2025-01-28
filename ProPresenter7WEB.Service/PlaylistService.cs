using ProPresenter7WEB.Core;
using System.Net.Http.Json;

namespace ProPresenter7WEB.Service
{
    public class PlaylistService : IPlaylistService
    {
        private readonly HttpClient _httpClient;
        private readonly IProPresenterService _proPresenterService;

        public PlaylistService(
            HttpClient httpClient,
            IProPresenterService proPresenterService)
        {
            _httpClient = httpClient;
            _proPresenterService = proPresenterService;
        }

        public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()
        {
            var response = await _httpClient.GetAsync($"{_proPresenterService.ApiAddress}/v1/playlists");

            response.EnsureSuccessStatusCode();

            var contracts = await response.Content.ReadFromJsonAsync<Contracts.Playlist[]>();

            if (contracts == null)
            {
                return Enumerable.Empty<Playlist>();
            }

            var playlists = FilterPlaylists(contracts);

            return playlists;
        }

        public async Task<PlaylistDetails> GetPlayListDetailsAsync(string uuid)
        {
            var response = await _httpClient.GetAsync($"{_proPresenterService.ApiAddress}/v1/playlist/{uuid}");

            response.EnsureSuccessStatusCode();

            var contract = await response.Content.ReadFromJsonAsync<Contracts.PlaylistDetails>();

            if (contract == null)
            {
                throw new InvalidOperationException($"No playlist details was found with uuid {uuid}.");
            }

            var playlistDetails = new PlaylistDetails
            {
                Uuid = contract.Id.Uuid,
                Name = contract.Id.Name,
                Presentations = contract.Items
                    .Where(item => item.Type == Contracts.PlaylistDetailsItemTypeEnum.Presentation)
                    .Select(item => new PlaylistDetailsPresentation
                    {
                        Uuid = item.Id.Uuid,
                        Name = item.Id.Name,
                    }),
            };

            return playlistDetails;
        }

        private IEnumerable<Playlist> FilterPlaylists(IEnumerable<Contracts.Playlist> contracts)
        {
            var playlists = new List<Playlist>();

            foreach (var contract in contracts)
            {
                if (contract.FieldType == Contracts.PlaylistFieldTypeEnum.Playlist)
                {
                    playlists.Add(new Playlist
                    {
                        Uuid = contract.Id.Uuid,
                        Name = contract.Id.Name,
                    });
                }
                else
                {
                    playlists.AddRange(FilterPlaylists(contract.Children));
                }
            }

            return playlists;
        }
    }
}
