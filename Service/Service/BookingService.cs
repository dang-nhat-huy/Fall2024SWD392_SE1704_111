using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.RequestDTO;
using BusinessObject.ResponseDTO;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
        }

        private async Task<bool> CheckExistService(int serviceId)
        {
                var service = await _unitOfWork.HairServiceRepository.GetByIdAsync(serviceId);
                return service != null;
        }

        private async Task<bool> CheckExistStylist(int stylistId)
        {
            var stylist = await _unitOfWork.HairServiceRepository.GetByIdAsync(stylistId);
            return stylist != null;
        }

        //public async Task<ResponseDTO> AddBooking(RequestDTO.AddToBookingDTO request)
        //{
        //    bool checkService =CheckExistService(request.)
        //    if (CheckExistService == false && CheckExistStylist == null)
        //    {
        //        return new ResponseDTO(Const.FAIL_CREATE_CODE, "The username is already taken. Please choose a different username.");
        //    }

        //    try
        //    {
        //        var booking = _mapper.Map<Booking>(request);

        //        booking.CreateDate = DateTime.UtcNow;

        //        await _unitOfWork.bookingRepository.CreateAsync(booking);
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //}
    }
}
