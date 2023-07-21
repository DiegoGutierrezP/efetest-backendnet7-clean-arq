using EfeTest.Backend.Application.DTOs.Request;
using EfeTest.Backend.Application.DTOs.Response;
using EfeTest.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<User> Register(RegisterUserRequest request);
        Task<LoginResponse> VerifyAndGenerateToken(RefreshTokenRequest request);
        Task<RefreshTokenResponse> GenerateTokenAndRefresh(int id, string email);
    }
}
