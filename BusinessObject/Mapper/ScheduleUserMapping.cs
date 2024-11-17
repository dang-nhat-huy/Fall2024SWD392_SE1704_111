using AutoMapper;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

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

            CreateMap<ScheduleUser, createScheduleUser>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap()
                .ForPath(dest => dest.Schedule.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForPath(dest => dest.Schedule.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForPath(dest => dest.Schedule.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForPath(dest => dest.Schedule.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ReverseMap();

            CreateMap<ScheduleUser, viewScheduleOfStylist>()
                .ForMember(dest => dest.ScheduleUserId, opt => opt.MapFrom(src => src.ScheduleUserId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap()
                .ForPath(dest => dest.User.UserProfile.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForPath(dest => dest.Schedule.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForPath(dest => dest.Schedule.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForPath(dest => dest.Schedule.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForPath(dest => dest.Schedule.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ReverseMap();

            CreateMap<ScheduleUser, getStartDateAndStartTime>()
                .ReverseMap()
                .ForPath(dest => dest.Schedule.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForPath(dest => dest.Schedule.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ReverseMap();

            CreateMap<User, StylistOfScheduleUserResponseDTO>()
                .ForMember(dest => dest.StylistId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.StylistName, opt => opt.MapFrom(src => src.UserProfile.FullName)) // Ensure this is correctly mapped
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap();
        }
    }
}
