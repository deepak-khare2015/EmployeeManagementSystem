using DAL.Entities;
using EmployeeManagement.Application.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace EmployeeDBFirst_Library.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected  readonly DbSet<T> _dbset;

        public GenericRepository(AppDbContext context)
        {

            _context = context;
            _dbset = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T Entity)
        {
            if (Entity == null) throw new ArgumentNullException(nameof(Entity));

            await _dbset.AddAsync(Entity);
            await _context.SaveChangesAsync();
            return Entity;

        }


        public virtual async Task UpdateAsync(T Entity)
        {

            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity));

            _context.Entry(Entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public virtual async Task DeleteAsyncs(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var entity = await _dbset.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException(nameof(id));

            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
