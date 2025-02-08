namespace ProPresenter7WEB.Service
{
    public interface IPresentationStorageService
    {
        void SetPresentationUuid(string uuid);
        string? GetPresentationUuid();
        void RemovePresentationUuid();
    }
}
