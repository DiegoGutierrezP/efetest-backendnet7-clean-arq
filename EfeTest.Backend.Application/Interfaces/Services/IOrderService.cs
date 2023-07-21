using EfeTest.Backend.Application.DTOs.Request;
using EfeTest.Backend.Domain.Entities;


namespace EfeTest.Backend.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(int userId, OrderCreateRequest reqData);
    }
}
