using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task<Feedback?> GetFeedbackById(int feedbackId);
        Task<int> CreateFeedbackAsync(Feedback entity);
        Task<int> UpdateFeedbackAsync(Feedback feedback);
    }
}
