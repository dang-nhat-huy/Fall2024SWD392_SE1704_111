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

namespace Service.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jWTService;
        private readonly IConfiguration _config;
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jWTService = jWTService;
            _config = config;

        }
        public Task<ResponseDTO> ChangeFeedbackStatus(RequestDTO.ChangefeedbackStatusDTO request, int feedbackId)
        {
            throw new NotImplementedException();
        }
        

        public async Task<ResponseDTO> CreateFeedback(RequestDTO.FeedbackRequestDTO feedbackRequest)
        {
            try
            {
                // Lấy người dùng hiện tại
                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_CREATE_CODE, "User not logged in.");
                }

                // Tạo một đối tượng Feedback mới từ DTO
                var feedback = _mapper.Map<Feedback>(feedbackRequest);
                feedback.UserId = user.UserId; // Gán UserId từ người dùng đã đăng nhập
                feedback.CreateDate = DateTime.Now;
                feedback.Status = (int?)FeedbackStatusEnum.Inactive;

                // Các thuộc tính khác của feedback (nếu có)

                // Lưu Feedback vào database thông qua UnitOfWork
                var checkUpdate = await _unitOfWork.FeedbackRepository.CreateFeedbackAsync(feedback);
                if (checkUpdate <= 0)
                {
                    return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }

                return new ResponseDTO(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, feedback);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, ex);
            }
        }

        public async Task<bool> DeleteFeedbackAsync( int feedbackId)
        {

            try
            {
                var feedback = await _unitOfWork.FeedbackRepository.GetFeedbackByIdAsync(feedbackId);
                if (feedback == null) return false;

                return await _unitOfWork.FeedbackRepository.DeleteFeedbackAsync(feedback);
            }
            catch 
            {
                return false;
            }
            
        }

        public async Task<ResponseDTO> GetAllFeedbacksAsync()
        {
            try
            {

                var listFeedback = await _unitOfWork.FeedbackRepository.GetAllFeedbackAsync();
                if (listFeedback == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "No Bookings found.");
                }
                else
                {
                    var result = _mapper.Map<List<FeedbackResponseDTO>>(listFeedback);
                    return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public Task<ResponseDTO> GetFeedbackHistoryOfCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}
