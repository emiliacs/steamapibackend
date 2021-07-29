using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using RyhmatyoBuuttiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegistrationDTO, User>();
            CreateMap<User, UserAuthenticateResponse>();
            CreateMap<JsonPatchDocument<UserUpdateDTO>, JsonPatchDocument<User>>();
            CreateMap<Operation<UserUpdateDTO>, Operation<User>>();
        }
    }
}
