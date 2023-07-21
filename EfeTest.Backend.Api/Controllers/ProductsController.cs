using EfeTest.Backend.Api.Filters;
using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EfeTest.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var products = await _unitOfWork.Product.GetAllAsync();
            return Ok(new { msg = "", data = products });
        }

    }
}
