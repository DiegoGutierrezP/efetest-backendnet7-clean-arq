using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace EfeTest.Backend.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        internal DbSet<T> DbSet { get; set; }
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }
        public async Task<T> AddEntity(T entity)
        {
            var result = await DbSet.AddAsync(entity);
            return result.Entity;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
