using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class FeedbackRepository : GenericRepository<Feedback>,IFeedbackRepository
    {
        public FeedbackRepository() { }
        public FeedbackRepository(HairSalonBookingContext context) => _context = context;
        public async Task<int> CreateFeedbackAsync(Feedback entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Feedback>> GetAllFeedbackAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(u => u.FeedbackId == id);
        }

        public Task<List<Feedback>> GetFeedbackHistoryByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

    
    }
}
