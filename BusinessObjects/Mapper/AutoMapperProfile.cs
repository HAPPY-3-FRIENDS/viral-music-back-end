using AutoMapper;
using BusinessObjects.DataTranferObjects;
using BusinessObjects.Models;
using System.Collections.Generic;

namespace BusinessObjects.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}