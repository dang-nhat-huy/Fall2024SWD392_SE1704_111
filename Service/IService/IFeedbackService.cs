using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Service.IService
{
    public interface IFeedbackService
    {
        Task<ResponseDTO> ChangeFeedbackStatus(ChangefeedbackStatusDTO request, int feedbackId);

        Task<ResponseDTO> CreateFeedback(FeedbackRequestDTO feedbackRequest);

        Task<ResponseDTO> GetFeedbackHistoryOfCurrentUser();
        Task<ResponseDTO> GetAllFeedbacksAsync();
        Task<bool> DeleteFeedbackAsync(int feedbackId);
    }
}
