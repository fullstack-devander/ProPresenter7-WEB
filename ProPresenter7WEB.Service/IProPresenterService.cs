using ProPresenter7WEB.Core;

namespace ProPresenter7WEB.Service
{
    public interface IProPresenterService
    {
        string? ApiAddress { get; }

        void SetApiAddress(string address, int port);
    }
}
