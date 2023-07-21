using EfeTest.Backend.Api.Filters;
using EfeTest.Backend.Application.DTOs.Request;
using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Application.Interfaces.Services;
using EfeTest.Backend.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EfeTest.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(IOrderService _orderService, IUnitOfWork _unitOfWork)
        {
            this._orderService = _orderService;
            this._unitOfWork = _unitOfWork;
        }

        [HttpGet]
        [Authorize(Role.Admin)]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _unitOfWork.Order.GetAllWithPopulate();
            return Ok(new { msg = "", data = orders });
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize(Role.Merchant,Role.Admin)]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _unitOfWork.Order.GetByIdWithPopulate(id);
            return Ok(new { msg = "", data = order });
        }

        [HttpGet]
        [Route("User/{userId?}")]
        [Authorize(Role.Merchant, Role.Admin)]
        public async Task<IActionResult> GetByUserId(int? userId)
        {
            int userIdReq = (int)(HttpContext.Items["UserId"] ?? throw new Exception("Usuario no autenticado"));
            var orders = await _unitOfWork.Order.GetByUserId(userId ?? userIdReq);
            return Ok(new { msg = "", data = orders });
        }

        [HttpPost]
        [Authorize(Role.Merchant, Role.Admin)]
        public async Task<IActionResult> Create(OrderCreateRequest request)
        {
            int userId = (int)(HttpContext.Items["UserId"] ?? throw new Exception("Usuario no autenticado"));
            var order = await _orderService.CreateOrder(userId, request);
            return Ok(new { msg = "Order creada correctamente", data = order });
        }
    }
}
