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

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserProfile)  // Include related data, if any
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<Feedback?> GetFeedbackById(int feedbackId)
        {
            return await _context.Feedbacks
                .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);
        }

        public async Task<int> CreateFeedbackAsync(Feedback entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateFeedbackAsync(Feedback feedback)
        {
            var tracker = _context.Attach(feedback);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }
    }
}
