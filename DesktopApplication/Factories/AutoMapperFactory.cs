using AutoMapper;
using ProPresenter7WEB.Service;

namespace ProPresenter7WEB.DesktopApplication.Factories
{
    public class AutoMapperFactory : IAutoMapperFactory
    {
        public IMapper Create()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMapperProfile>();
            }).CreateMapper();
        }
    }
}
