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
    public class ServiceStylistMapping : Profile
    {
        public ServiceStylistMapping()
        {
            CreateMap<ServicesStylist, StylistResponseDTO>()
                .ForPath(dest => dest.StylistName, opt => opt.MapFrom(src => src.Stylist.UserName))
                .ReverseMap();

        }
    }
}
