using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRefreshTokenRepository RefreshToken { get; private set; }
        public IUserRepository User { get; private set; }
        public IProductRepository Product { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IOrderItemRepository OrderItem { get; private set; }

        public UnitOfWork(AppDbContext _context)
        {
            this._context = _context;
            this.RefreshToken = new RefreshTokenRepository(_context);
            this.User = new UserRepository(_context);
            this.Product = new ProductRepository(_context);
            this.Order = new OrderRepository(_context);
            this.OrderItem = new OrderItemRepository(_context);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            _context.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
