namespace ProPresenter7WEB.Service
{
    public class ProPresenterService : IProPresenterService
    {
        public string? ApiAddress { get; private set; }

        public void SetApiAddress(string address, int port)
        {
            ApiAddress = $"http://{address}:{port}";
        }
    }
}
