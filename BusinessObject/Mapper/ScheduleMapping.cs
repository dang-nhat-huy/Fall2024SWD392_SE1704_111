using AutoMapper;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ReportEnum;
using static BusinessObject.RequestDTO.RequestDTO;

namespace BusinessObject.Mapper
{
    public class ScheduleMapping : Profile
    {
        public ScheduleMapping()
        {
            CreateMap<CreateScheduleDTO, Schedule>()
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ScheduleEnum.Available)) // Đặt giá trị mặc định cho Status
               .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now)) // Đặt ngày tạo mặc định
               .ForMember(dest => dest.CreateBy, opt => opt.Ignore())
               .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
               .ForMember(dest => dest.UpdateBy, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<UpdateScheduleDTO, Schedule>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now)) // Đặt ngày tạo mặc định
                .ForMember(dest => dest.CreateBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateBy, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Schedule, ScheduleDTO>()
                .ReverseMap();
        }


    }
}
