using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateRefreshToken(int idUser);
        string GenerateToken(int idUser, string emailUser);
        int? VerifyToken(string token);
    }
}
