namespace ProPresenter7WEB.Service
{
    public class PresentationStorageService : IPresentationStorageService
    {
        private string? _presentationUuid;

        public void RemovePresentationUuid()
        {
            _presentationUuid = null;
        }

        public void SetPresentationUuid(string uuid)
        {
            _presentationUuid = uuid;
        }

        public string? GetPresentationUuid()
        {
            return _presentationUuid;
        }
    }
}
