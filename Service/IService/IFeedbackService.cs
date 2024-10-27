using BusinessObject;
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
        Task<ResponseDTO> GetFeedbackListAsync();
        Task<ResponseDTO> CreateFeedbackAsync(FeedbackRequestDTO request);
        Task<ResponseDTO> UpdateFeedbackAsync(FeedbackRequestDTO request, int feedbackId);
        Task<ResponseDTO> ChangeFeedbackStatusAsync(int feedbackId, FeedbackStatusEnum status);
    }
}
