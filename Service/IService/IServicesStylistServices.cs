﻿using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IServicesStylistServices
    {
        Task<ResponseDTO> GetStylistsByServiceIdAsync(int serviceId);
    }
}
