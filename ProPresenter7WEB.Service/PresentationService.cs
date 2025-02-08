using ProPresenter7WEB.Core;

namespace ProPresenter7WEB.Service
{
    public class PresentationService : IPresentationService
    {
        public Presentation[] GetPresentations()
        {
            return [
                new Presentation
                {
                    Id = 1,
                    Name = "Test",
                },
                new Presentation
                {
                    Id = 2,
                    Name = "Test",
                }
            ];
        }
    }
}
