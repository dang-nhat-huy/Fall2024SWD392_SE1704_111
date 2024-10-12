using AutoMapper;
using BusinessObject.Models;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Mapper
{
    public class UserProfileMaping : Profile
    {
        public UserProfileMaping() {
            CreateMap<UserProfile, UserProfileDTO>().ReverseMap();
        }
       
    }
}
