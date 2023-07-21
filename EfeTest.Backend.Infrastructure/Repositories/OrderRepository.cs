using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Domain.Entities;
using EfeTest.Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Order>> GetAllWithPopulate()
        {
            return await DbSet.Include(x => x.User).Include(x => x.Items).ToListAsync();
        }

        public async Task<Order?> GetByIdWithPopulate(int id)
        {
            return await DbSet.Where(x => x.Id == id).Include(x => x.User).Include(x => x.Items).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetByUserId(int userId)
        {
            return await DbSet.Where(x => x.UserId == userId).Include(x => x.User).Include(x => x.Items).ToListAsync();
        }
    }
}
