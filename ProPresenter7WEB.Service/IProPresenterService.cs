using ProPresenter7WEB.Core;

namespace ProPresenter7WEB.Service
{
    public interface IProPresenterService
    {
        void SetProPresenterConnection(string address, int port);

        Task<ProPresenterInfo> GetProPresenterInfoAsync();
    }
}
