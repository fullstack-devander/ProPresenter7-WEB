using Microsoft.AspNetCore.Mvc;
using ProPresenter7WEB.Core;
using ProPresenter7WEB.Service;

namespace ProPresenter7WEB.WebServerApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresentationController : ControllerBase
    {
        private readonly IPresentationService _presentationService;

        public PresentationController(IPresentationService presentationService)
        {
            _presentationService = presentationService;
        }

        [HttpGet]
        public IEnumerable<Presentation> Get()
        {
            var result = _presentationService.GetPresentations();

            return result;
        }
    }
}
