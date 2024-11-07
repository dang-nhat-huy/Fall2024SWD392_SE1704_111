using AutoMapper;
using BusinessObject;
using BusinessObject.RequestDTO;
using BusinessObject.ResponseDTO;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;
using static BusinessObject.FeedbackStatusEnum;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BusinessObject.Paging;
using static BusinessObject.VoucherEnum;

namespace Service.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTService _jWTService;
        public FeedbackService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jWTService = jWTService;
        }
        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        }
        public async Task<ResponseDTO> CreateFeedbackAsync(CreateFeedbackDTO request)
        {
            try
            {
                //AutoMapper from RegisterRequestDTO => User
                var feedback = _mapper.Map<Feedback>(request);

                feedback.CreateDate = DateTime.Now;
                feedback.Status = FeedbackStatusEnum.Active;
                feedback.Description = request.Description;
              

                await _unitOfWork.FeedbackRepository.CreateAsync(feedback);
                return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "Feedback created successfully", request);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> UpdateFeedbackAsync(FeedbackRequestDTO request, int feedbackId)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Token is missing.");
                }

                var claimsPrincipal = ValidateToken(token);

                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found in token.");
                }

                var feedback = await _unitOfWork.FeedbackRepository.GetFeedbackById(feedbackId);
                if (feedback == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Feedback not found");
                }

                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                _mapper.Map(request, feedback);

                feedback.UpdateBy = user.UserName;
                feedback.UpdateDate = DateTime.Now;

                var result = await _unitOfWork.FeedbackRepository.UpdateFeedbackAsync(feedback);

                if (result == null)
                {
                    return new ResponseDTO(Const.FAIL_UPDATE_CODE, "Update feedback failed");
                }

                return new ResponseDTO(Const.SUCCESS_UPDATE_CODE, "Update feedback succeeded");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> ChangeFeedbackStatusAsync(int feedbackId, FeedbackStatusEnum newStatus)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Token is missing.");
                }

                var claimsPrincipal = ValidateToken(token);

                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found in token.");
                }

                var feedback = await _unitOfWork.FeedbackRepository.GetFeedbackById(feedbackId);
                if (feedback == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Feedback not found");
                }

                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                feedback.Status = newStatus;
                feedback.UpdateBy = user.UserName;
                feedback.UpdateDate = DateTime.Now;

                var result = await _unitOfWork.FeedbackRepository.UpdateFeedbackAsync(feedback);

                if (result == null)
                {
                    return new ResponseDTO(Const.FAIL_UPDATE_CODE, "Update feedback status failed");
                }

                return new ResponseDTO(Const.SUCCESS_UPDATE_CODE, "Update feedback status succeeded");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> GetFeedbackListAsync()
        {
            try
            {
                // Retrieve all feedbacks from the repository
                var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync();

                if (feedbacks == null || !feedbacks.Any())
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Empty Feedback List");
                }

                // Map feedback entities to FeedbackDTOs
                var feedbackDTOs = _mapper.Map<List<FeedbackResponseDTO>>(feedbacks);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, feedbackDTOs);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<PagedResult<Feedback>> GetAllFeedbackPagingAsync(int pageNumber, int pageSize)
        {
            try
            {
                var feedbackList = _unitOfWork.FeedbackRepository.GetAll().Where(s => s.Status == FeedbackStatusEnum.Active);
                if (feedbackList == null)
                {
                    throw new Exception();
                }
                return await Paging.GetPagedResultAsync(feedbackList.AsQueryable(), pageNumber, pageSize);
            }
            catch (Exception)
            {
                return new PagedResult<Feedback>();
            }
        }

        public async Task<ResponseDTO> GetFeedbackByIdAsync(int feedbackId)
        {
            try
            {

                var feedback = await _unitOfWork.FeedbackRepository.GetFeedbackById(feedbackId);

                // Kiểm tra nếu danh sách rỗng
                if (feedback == null)
                {
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "No feedbacks found with the ID");
                }

                // Sử dụng AutoMapper để ánh xạ các entity sang DTO
                var result = _mapper.Map<FeedbackResponseDTO>(feedback);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu xảy ra
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> ChangeStatusFeedbackById(int feedbackId)
        {
            try
            {
                // Lấy người dùng hiện tại
                var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(feedbackId);
                if (feedback == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, "Feedback not found !");
                }

                // Sử dụng AutoMapper để ánh xạ thông tin từ DTO vào user

                feedback.Status = feedback.Status == FeedbackStatusEnum.Active ? FeedbackStatusEnum.Inactive : FeedbackStatusEnum.Active;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.FeedbackRepository.UpdateAsync(feedback);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, "Change Feedback Status Succeed");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<PagedResult<Feedback>> SearchFeedbackByDescriptionAsync(string query, int pageNumber, int pageSize)
        {
            try
            {
                var feedbacks = await _unitOfWork.FeedbackRepository.GetAll()
 .Where(f => f.Description.Contains(query))
 .Skip((pageNumber - 1) * pageSize)
 .Take(pageSize)
 .ToListAsync();

                var totalFeedbacks = await _unitOfWork.FeedbackRepository.GetAll()
                    .CountAsync(f => f.Description.Contains(query));

                return new PagedResult<Feedback>
                {
                    Items = feedbacks,
                    TotalCount = totalFeedbacks,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

            }
            catch (Exception ex)
            {
                return new PagedResult<Feedback>();
            }

        }

        public async Task<PagedResult<Feedback>> GetAllFeedbackPagingAsync1(int pageNumber, int pageSize)
        {
            try
            {
                var feedbackList = _unitOfWork.FeedbackRepository.GetAll();
                if (feedbackList == null)
                {
                    throw new Exception();
                }
                return await Paging.GetPagedResultAsync(feedbackList.AsQueryable(), pageNumber, pageSize);
            }
            catch (Exception)
            {
                return new PagedResult<Feedback>();
            }
        }
    }
}
