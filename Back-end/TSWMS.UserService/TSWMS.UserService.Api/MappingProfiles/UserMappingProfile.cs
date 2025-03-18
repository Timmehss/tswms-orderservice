﻿using AutoMapper;
using TSWMS.UserService.Api.Dto;
using TSWMS.UserService.Shared.Models;

namespace TSWMS.UserService.Shared.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}
