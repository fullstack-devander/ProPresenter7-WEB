using AutoMapper;
using ProPresenter7WEB.Core;
using ProPresenter7WEB.Service.Contracts;

namespace ProPresenter7WEB.Service
{
    public class ServiceMapperProfile : Profile
    {
        public ServiceMapperProfile()
        {
            CreateMap<VersionInfo, ProPresenterInfo>();

            CreateMap<Contracts.Playlist, Core.Playlist>()
                .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id.Uuid))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Id.Name));

            CreateMap<PlaylistDetailsItem, PlaylistDetailsPresentation>()
                .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id.Uuid))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Id.Name));
            
            CreateMap<Contracts.PlaylistDetails, Core.PlaylistDetails>()
                .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id.Uuid))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Id.Name))
                .ForMember(dest => dest.Presentations, opt => opt.MapFrom(src => 
                    src.Items.Where(item => item.Type == PlaylistDetailsItemTypeEnum.Presentation)));
        }
    }
}
