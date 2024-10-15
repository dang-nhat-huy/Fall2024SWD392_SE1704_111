using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.VoucherEnum;

namespace Service.Service
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VoucherService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDTO> GetListVoucherAsync()
        {
            try
            {
                var listVoucher = await _unitOfWork.VoucherRepository.GetVocherByStatus(VoucherStatusEnum.Active);

                var voucherDto = _mapper.Map<List<VoucherDTO>>(listVoucher);

                if (voucherDto == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "No voucher found.");
                }
                else
                {
                    return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, voucherDto);
                }

            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
