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
    public class ScheduleMapping : Profile
    {
        public ScheduleMapping()
        {
            CreateMap<Schedule, ScheduleDTO>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Bookings.Select(b => b.CustomerId)))
                .ReverseMap();
        }
    }
}
