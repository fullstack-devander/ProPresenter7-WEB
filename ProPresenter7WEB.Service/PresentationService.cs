using ProPresenter7WEB.Core;

namespace ProPresenter7WEB.Service
{
    public class PresentationService : IPresentationService
    {
        private readonly ISharedService _sharedService;

        public PresentationService(ISharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public Presentation[] GetPresentations()
        {
            return [
                new Presentation
                {
                    Id = 1,
                    Name = _sharedService.Data,
                },
                new Presentation
                {
                    Id = 2,
                    Name = _sharedService.Data,
                }
            ];
        }
    }
}
