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
            CreateMap<ScheduleUser, ScheduleUserDTO>().ReverseMap();
        }
    }
}
