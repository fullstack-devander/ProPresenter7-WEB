using ProPresenter7WEB.Core;
using ProPresenter7WEB.Service.Contracts;
using System.Net.Http.Json;

namespace ProPresenter7WEB.Service
{
    public class ProPresenterService : IProPresenterService
    {
        private string? _hostAddress;

        private readonly HttpClient _httpClient;

        public ProPresenterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetProPresenterConnection(string address, int port)
        {
            _hostAddress = $"http://{address}:{port}";
        }

        public async Task<ProPresenterInfo> GetProPresenterInfoAsync()
        {
            var response = await _httpClient.GetAsync($"{_hostAddress}/version");

            response.EnsureSuccessStatusCode();

            var contract = await response.Content.ReadFromJsonAsync<VersionInfo>();

            if (contract == null)
            {
                throw new InvalidOperationException("Response cannot be deserialized.");
            }

            return new ProPresenterInfo
            {
                ApiVersion = contract.ApiVersion,
                HostDescription = contract.HostDescription,
                Name = contract.Name,
                OsVersion = contract.OsVersion,
                Platform = contract.Platform,
            };
        }
    }
}
