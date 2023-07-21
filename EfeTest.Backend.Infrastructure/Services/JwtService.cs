using EfeTest.Backend.Application.DTOs;
using EfeTest.Backend.Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtModel jwtConfig;
        public JwtService(IOptions<JwtModel> config)
        {
            jwtConfig = config.Value;
        }

        public string GenerateRefreshToken(int idUser)
        {
            string refresToken = RandomString(35) + Guid.NewGuid() + "_" + idUser;
            return refresToken;
        }

        public string GenerateToken(int idUser, string emailUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", idUser.ToString()) ,
                    new Claim(JwtRegisteredClaimNames.Email, emailUser) ,
                }),
                Expires = DateTime.UtcNow.AddDays(jwtConfig.ValidateInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? VerifyToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
            try
            {
                var tokenInVerification = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                var tokenClaim = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);

                if (tokenClaim == null) return null;

                var utcExpiryDate = long.Parse(tokenClaim.Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                //if (expiryDate.AddMinutes(1) < DateTime.UtcNow)//inactividad
                if (expiryDate < DateTime.UtcNow)
                {
                    return null;
                }

                var userId = int.Parse(tokenInVerification.Claims.First(x => x.Type == "Id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }
        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
