﻿using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ReportDTO;

namespace Service.IService
{
    public interface IBookingDetailService
    {
        Task<BookingDetailResponseDTO> GetBookingDetailByIdAsync(int bookingDetailID);
        Task<ResponseDTO> GetBookingOfCurrentStylist();
    }
}
