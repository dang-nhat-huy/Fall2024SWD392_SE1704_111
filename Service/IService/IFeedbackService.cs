using BusinessObject;
using BusinessObject.Model;
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
        Task<PagedResult<Feedback>> GetAllFeedbackPagingAsync(int pageNumber, int pageSize);
        Task<ResponseDTO> GetFeedbackByIdAsync(int feedbackId);
        Task<ResponseDTO> ChangeStatusFeedbackById(int feedbackId);
        Task<PagedResult<Feedback>> SearchFeedbackByDescriptionAsync(string query, int pageNumber, int pageSize);
    }
}
