using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;
using AutoMapper;

namespace BusinessObject.Mapper
{
    public class BookingDetailMapping : Profile
    {
        public BookingDetailMapping()
        {
            CreateMap<BookingDetail, BookingDetailResponseDTO>()
    .ForMember(dest => dest.BookingDetailID, opt => opt.MapFrom(src => src.BookingDetailId))
    .ForMember(dest => dest.BookingID, opt => opt.MapFrom(src => src.BookingId))
    .ForMember(dest => dest.StylistID, opt => opt.MapFrom(src => src.StylistId))
    .ForMember(dest => dest.ServiceID, opt => opt.MapFrom(src => src.ServiceId))
    .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
    .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy))
    .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
    .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdateBy));
        }
    }
}
