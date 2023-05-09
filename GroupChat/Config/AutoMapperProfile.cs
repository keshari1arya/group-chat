using AutoMapper;
using GroupChat.Dto;
using GroupChat.Models;

namespace GroupChat.Config;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserRequest, User>();
        CreateMap<GroupRequest, Group>();

        CreateMap<GroupMessage, GroupMessageResponse>()
        .ForMember(dest => dest.LikedByUsers, opt => opt.MapFrom(src => src.MessageLikes.Select(x => x.Id)));

        CreateMap<User, UserResponse>();
    }
}
