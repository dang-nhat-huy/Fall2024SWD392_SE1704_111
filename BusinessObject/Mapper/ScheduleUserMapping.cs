using AutoMapper;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Mapper
{
    public class ScheduleUserMapping : Profile
    {
        public ScheduleUserMapping()
        {
            CreateMap<ScheduleUser, ScheduleUserDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Schedule.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Schedule.EndTime))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Schedule.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Schedule.EndDate));


            CreateMap<ScheduleCurrentUserDTO, ScheduleUser>()
                .ForMember(dest => dest.Schedule, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<ScheduleUser, ScheduleCurrentUserDTO>();
        }
    }
}
