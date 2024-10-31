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
    public class BookingMapping : Profile
    {
        public BookingMapping()
        {
            CreateMap<Booking, ViewBookingDTO>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Voucher, opt => opt.MapFrom(src => src.Voucher))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments))
                .ForMember(dest => dest.BookingDetails, opt => opt.MapFrom(src => src.BookingDetails))
                .ReverseMap();

            CreateMap<Booking, BookingRequestDTO>()
                .ForMember(dest => dest.VoucherId, opt => opt.MapFrom(src => src.VoucherId))
                .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                .ReverseMap();


            CreateMap<Booking, ChangebookingStatusDTO>().ReverseMap();

            CreateMap<Booking, BookingHistoryDTO>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Schedule.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Schedule.EndDate))
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src =>
                        src.BookingDetails.Select(bookingDetail => new ServiceDetailDTO
                        {
                        ServiceId = bookingDetail.ServiceId ?? 0, 
                        ServiceName = bookingDetail.Service.ServiceName, 
                        StylistName = bookingDetail.Stylist.UserName 
            }).ToList()))
            .ReverseMap();

            CreateMap<Booking, BookingResponseDTO>()
                 .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                 .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                 .ForMember(dest => dest.VoucherId, opt => opt.MapFrom(src => src.VoucherId))
                 .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId))
                 .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                 .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.StaffId))
                 .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                 .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                 .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdateBy))
                 .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
                 .ReverseMap();
        }
    }
}
