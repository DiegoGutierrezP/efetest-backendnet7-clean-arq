using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Domain.Entities;
using EfeTest.Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EfeTest.Backend.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await DbSet.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }

    }
}
