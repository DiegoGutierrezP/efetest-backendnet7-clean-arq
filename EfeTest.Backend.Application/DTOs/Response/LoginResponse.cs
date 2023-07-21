using EfeTest.Backend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Application.DTOs.Response
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Role Rol { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
