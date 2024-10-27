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
        Task<Feedback> GetFeedbackByIdAsync(int id);
        Task<int> CreateFeedbackAsync(Feedback entity);
        Task<List<Feedback>> GetFeedbackHistoryByCustomerIdAsync(int customerId);
        Task<List<Feedback>> GetAllFeedbackAsync();
        Task<bool> DeleteFeedbackAsync(Feedback entity);
    }
}
