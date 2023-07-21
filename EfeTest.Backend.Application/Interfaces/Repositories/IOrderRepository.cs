using EfeTest.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetByIdWithPopulate(int id);
        Task<List<Order>> GetAllWithPopulate();
        Task<List<Order>> GetByUserId(int userId);
    }
}
