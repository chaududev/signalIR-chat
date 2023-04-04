using AutoMapper;
using ChatRoom.Models;
using ChatRoom.ViewModels;

namespace ChatRoom.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(x => x.UserName));

            CreateMap<UserViewModel, User>();
        }
    }
}
