using EfeTest.Backend.Application.DTOs.Request;
using EfeTest.Backend.Application.DTOs.Response;
using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Application.Interfaces.Services;
using EfeTest.Backend.Domain.Entities;
using EfeTest.Backend.Infrastructure.Helpers;

namespace EfeTest.Backend.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IJwtService _jwtService, IUnitOfWork _unitOfWork)
        {
            this._jwtService = _jwtService;
            this._unitOfWork = _unitOfWork; 
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _unitOfWork.User.GetByEmail(request.Email);

            if (user == null) throw new Exception("Usuario no encontrado");

            if (Encriptacion.Desencriptar(user.Password) != request.Password)
            {
                throw new Exception("Contraseña incorrecta");
            }

            LoginResponse loginRes = new LoginResponse();
            loginRes.Id = user.Id;
            loginRes.Email = user.Email;
            loginRes.Name = user.Name + " "+ user.FullName;
            loginRes.Rol = user.Rol;

            var tokens = await GenerateTokenAndRefresh(user.Id, user.Email);
            loginRes.Token = tokens.Token;
            loginRes.RefreshToken = tokens.RefreshToken;

            return loginRes;
        }

        public async Task<User> Register(RegisterUserRequest request)
        {
            var user = await _unitOfWork.User.GetByEmail(request.Email);

            if (user != null) throw new Exception("El email ya esta registrado");

            User newUser = new User();
            newUser.Name = request.Name;
            newUser.Email = request.Email;
            newUser.Password = Encriptacion.Encriptar(request.Password);
            newUser.FullName = request.FullName;
            newUser.Rol = Domain.Enums.Role.Merchant;
            var userCreated = await _unitOfWork.User.AddEntity(newUser);
            await _unitOfWork.CommitAsync();

            return userCreated;
        }

        public async Task<LoginResponse> VerifyAndGenerateToken(RefreshTokenRequest request)
        {
            var userId = _jwtService.VerifyToken(request.Token);

            LoginResponse loginResRf = new LoginResponse();

            if (userId == null)
            {
                var storedToken = await _unitOfWork.RefreshToken.GetByToken(request.RefreshToken);

                if (storedToken == null) throw new Exception("RefreshToken does not exist");
                if (storedToken.IsUsed) throw new Exception("RefreshToken has been used");
                if (storedToken.ExpiryDate < DateTime.UtcNow) throw new Exception("RefreshToken has expired");

                storedToken.IsUsed = true;
                _unitOfWork.RefreshToken.Update(storedToken);
                await _unitOfWork.CommitAsync();

                var userRf = await _unitOfWork.User.GetByIdAsync((int)storedToken.UserId);

                //generamos un nuevo token con refreshtoken

                loginResRf.Id = userRf.Id;
                loginResRf.Email = userRf.Email;
                loginResRf.Name = userRf.Name + " " + userRf.FullName;
                loginResRf.Rol = userRf.Rol;

                var tokens = await GenerateTokenAndRefresh(userRf.Id, userRf.Email);
                loginResRf.Token = tokens.Token;
                loginResRf.RefreshToken = tokens.RefreshToken;

                return loginResRf;
            }
            else
            {
                loginResRf.Token = request.Token;
                loginResRf.RefreshToken = request.RefreshToken;
            }

            return loginResRf;
        }

        public async Task<RefreshTokenResponse> GenerateTokenAndRefresh(int id, string email)
        {
            //TODO:GENERAR TOKEN Y REFRESH TOKEN
            string token = _jwtService.GenerateToken(id, email);
            string refreshToken = _jwtService.GenerateRefreshToken(id);

            await _unitOfWork.RefreshToken.AddEntity(new Domain.Entities.RefreshToken()
            {
                IsUsed = false,
                UserId = id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                Token = refreshToken
            });
            await _unitOfWork.CommitAsync();

            return new RefreshTokenResponse { Token = token, RefreshToken = refreshToken };
        }
    }
}

