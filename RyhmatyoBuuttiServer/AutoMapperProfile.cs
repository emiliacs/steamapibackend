using AutoMapper;
using RyhmatyoBuuttiServer.Models;

namespace RyhmatyoBuuttiServer
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegistrationDTO, User>();
            CreateMap<User, UserAuthenticateResponse>();

            CreateMap<User, UserDTO>();
            CreateMap<Friend, FriendDTO>();

           
 
        }
    }
}
