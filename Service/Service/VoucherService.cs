using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.Paging;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;
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

        public async Task<ResponseDTO> GetVoucherByIdAsync(int voucherId)
        {
            try
            {
                
                var voucher = await _unitOfWork.VoucherRepository.GetVoucherById(voucherId);

                // Kiểm tra nếu danh sách rỗng
                if (voucher == null)
                {
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "No vouchers found with the ID");
                }

                // Sử dụng AutoMapper để ánh xạ các entity sang DTO
                var result = _mapper.Map<VoucherDTO>(voucher);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu xảy ra
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> ChangeStatusVoucherById(int voucherId)
        {
            try
            {
                // Lấy người dùng hiện tại
                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(voucherId);
                if (voucher == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, "Voucher not found !");
                }

                // Sử dụng AutoMapper để ánh xạ thông tin từ DTO vào user

                voucher.Status = voucher.Status == VoucherStatusEnum.Active ? VoucherStatusEnum.Inactive : VoucherStatusEnum.Active;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.VoucherRepository.UpdateAsync(voucher);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Change Voucher Status Succeed");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> UpdateVoucherById(int voucherId, UpdateVoucherDTO request)
        {
            try
            {

                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(voucherId);
                if (voucher == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, "Voucher not found !");
                }

                // Sử dụng AutoMapper để ánh xạ thông tin từ DTO vào user
                _mapper.Map(request, voucher);

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.VoucherRepository.UpdateAsync(voucher);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Change voucher Status Succeed");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> CreateVoucherAsync(CreateVoucherDTO request)
        {
            try
            {
                //AutoMapper from RegisterRequestDTO => User
                var voucher = _mapper.Map<Voucher>(request);

                voucher.CreateDate = DateTime.UtcNow;
                voucher.Status = VoucherStatusEnum.Active;

                await _unitOfWork.VoucherRepository.CreateAsync(voucher);
                return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "Voucher created successfully", request);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<PagedResult<Voucher>> GetAllVoucherPagingAsync(int pageNumber, int pageSize)
        {
            try
            {
                var voucherList = _unitOfWork.VoucherRepository.GetAll();
                if (voucherList == null)
                {
                    throw new Exception();
                }
                return await Paging.GetPagedResultAsync(voucherList.AsQueryable(), pageNumber, pageSize);
            }
            catch (Exception)
            {
                return new PagedResult<Voucher>();
            }
        }
    }
}
