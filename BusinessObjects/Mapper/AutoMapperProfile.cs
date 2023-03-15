using AutoMapper;
using BusinessObjects.DataTranferObjects;
using BusinessObjects.Models;

namespace BusinessObjects.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dto => dto.RoleName, act => act.MapFrom(obj => obj.Role.RoleName))
                .ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Track, TrackDTO>().ReverseMap();
            CreateMap<Artist, ArtistDTO>().ReverseMap();
            CreateMap<TrackInPlaylist, TrackInPlaylistDTO>()
                .ForMember(dto => dto.Track, act => act.MapFrom(obj => obj.Track))
                .ReverseMap();
        }
    }
}