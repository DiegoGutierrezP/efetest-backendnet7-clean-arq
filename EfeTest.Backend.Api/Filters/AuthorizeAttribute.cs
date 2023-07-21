using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Application.Interfaces.Services;
using EfeTest.Backend.Domain.Entities;
using EfeTest.Backend.Domain.Enums;
using EfeTest.Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EfeTest.Backend.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public IList<Role>? Roles { get; set; }

        public AuthorizeAttribute(params Role[] Roles) : base(typeof(AuthorizeFilter))
        {
            this.Roles = Roles ?? new Role[] { };
        }
        public AuthorizeAttribute() : base(typeof(AuthorizeFilter))
        {

        }
    }
    public class AuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        public AuthorizeFilter(IJwtService jwtService, IUnitOfWork _unitOfWork)
        {
            this._jwtService = jwtService;
            this._unitOfWork = _unitOfWork;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            var authorize = context.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>();
            if (allowAnonymous || !authorize.Any())
                return;

            var attribute = authorize.First();

            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last() ?? throw new Exception("Se requiere un token de acceso");
            var userId = _jwtService.VerifyToken(token) ?? throw new Exception("Token no válido");
            // attach account to context on successful jwt validation


            var user = await _unitOfWork.User.GetByIdAsync(userId);


            bool permitido = false;

            //verificamos si tiene los roles
            if (attribute.Roles != null)
            {
                 if (attribute.Roles.Contains(user.Rol))
                  {
                        permitido = true;
                  }
            }
            else
            {
                permitido = true;
            }

            if (!permitido)
            {
                throw new Exception("El usuario no tiene el rol correcto");
            }

            context.HttpContext.Items["Token"] = token;
            context.HttpContext.Items["UserId"] = user.Id;
        }
    }

}
