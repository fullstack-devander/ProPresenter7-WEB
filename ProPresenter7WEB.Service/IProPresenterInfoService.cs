using ProPresenter7WEB.Core;

namespace ProPresenter7WEB.Service
{
    public interface IProPresenterInfoService
    {
        Task<ProPresenterInfo> GetProPresenterInfoAsync();
    }
}
