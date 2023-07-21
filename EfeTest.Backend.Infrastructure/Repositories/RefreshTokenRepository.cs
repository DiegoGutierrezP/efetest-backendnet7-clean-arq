using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Domain.Entities;
using EfeTest.Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace EfeTest.Backend.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> GetByToken(string token)
        {
            return await DbSet.Where(x => x.Token == token).FirstOrDefaultAsync();
        }
    }
}
