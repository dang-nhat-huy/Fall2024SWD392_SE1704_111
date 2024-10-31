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
            CreateMap<User, RegisterRequestDTO>()
                .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
                .ReverseMap()
                .ForPath(dest => dest.UserProfile.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForPath(dest => dest.UserProfile.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.UserProfile.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForPath(dest => dest.UserProfile.Address, opt => opt.MapFrom(src => src.Address))
                .ForPath(dest => dest.UserProfile.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth)).ReverseMap();

            CreateMap<User, LoginResponse>().ReverseMap();

            CreateMap<User, ChangeStatusAccountDTO>().ReverseMap();

            CreateMap<User, UserListDTO>().ReverseMap();

            CreateMap<User, SearchAccountByNameDTO>().ReverseMap();

            CreateMap<User, CreateAccountDTO>().ReverseMap();

            CreateMap<User, UpdateAccountDTO>()
                .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Role))
                .ReverseMap();
        }
        

    }
}
