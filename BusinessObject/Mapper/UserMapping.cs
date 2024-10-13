using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;
using AutoMapper;
using BusinessObject.ResponseDTO;
using static BusinessObject.ResponseDTO.UserProfileDTO;

namespace BusinessObject.Mapper
{
    public class UserMapping : Profile
    {
        public UserMapping() {
            CreateMap<User, RegisterRequestDTO>().ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.userName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.password))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone));

            CreateMap<User, LoginResponse>().ReverseMap();

            CreateMap<User, ChangeStatusAccountDTO>().ReverseMap();

            CreateMap<User, UserListDTO>().ReverseMap();

            CreateMap<User, SearchAccountByNameDTO>().ReverseMap();

            
        }
        

    }
}
