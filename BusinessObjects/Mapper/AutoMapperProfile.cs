using AutoMapper;
using BusinessObjects.DataTranferObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObjects.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dto => dto.RoleName, act => act.MapFrom(obj => obj.Role.RoleName)).ReverseMap();
        }
    }
}