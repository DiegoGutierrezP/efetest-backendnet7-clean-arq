using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IRefreshTokenRepository RefreshToken { get; }
        IUserRepository User { get; }
        IProductRepository Product { get; }
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get;  }

        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
