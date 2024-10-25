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

namespace Service.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jWTService;
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jWTService = jWTService;
        }
        public Task<ResponseDTO> ChangeFeedbackStatus(RequestDTO.ChangefeedbackStatusDTO request, int feedbackId)
        {
            throw new NotImplementedException();
        }
        

        public Task<ResponseDTO> CreateFeedback(RequestDTO.FeedbackRequestDTO feedbackRequest)
        {
            throw new NotImplementedException();
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
