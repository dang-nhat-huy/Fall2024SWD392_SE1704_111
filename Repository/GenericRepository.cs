using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;

namespace BadmintonRentingData.Base
{

    public class GenericRepository<T> where T : class
    {
        protected HairSalonBookingContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository()
        {
            _context = new HairSalonBookingContext();
            _dbSet = _context.Set<T>();
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            var list = await _dbSet.ToListAsync();
            return list;
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public bool Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(long? id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T GetByName(string name)
        {
            return _dbSet.Find(name);
        }

        public async Task<T> GetByNameAsync(string name)
        {
            return await _dbSet.FindAsync(name);
        }
    }
}
