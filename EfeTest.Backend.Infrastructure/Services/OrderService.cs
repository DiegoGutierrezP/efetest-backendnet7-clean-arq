using EfeTest.Backend.Application.DTOs.Request;
using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Application.Interfaces.Services;
using EfeTest.Backend.Domain.Entities;
using EfeTest.Backend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public async Task<Order> CreateOrder(int userId , OrderCreateRequest reqData)
         {
            if (reqData.Products.Count == 0) throw new Exception("Debe ingresar al menos un producto");
            if(string.IsNullOrEmpty(reqData.ShippingAddress)) throw new Exception("Debe ingresar la direccion de envio");

            double totalAmount = 0;
            int index = 0;
            foreach (var p in reqData.Products)
            {
                var pro = await _unitOfWork.Product.GetByIdAsync(p.ProductId);
                if(pro != null)
                {
                    p.ProductName = pro.Name;
                    p.TotalPrice = (pro.Price * p.Quantity);
                    totalAmount += (pro.Price * p.Quantity);
                }
                else
                {
                    reqData.Products.RemoveAt(index);
                }
                index++;
            }

            if(reqData.Products.Count == 0) throw new Exception("Porfavor ingrese al menos un producto valido");

            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.OrderDate = DateTime.Now;
            newOrder.Status = OrderStatus.Pendiente;
            newOrder.ShippingAddress = reqData.ShippingAddress;
            newOrder.TotalAmount = totalAmount;

            var orderCreated = await _unitOfWork.Order.AddEntity(newOrder);
            await _unitOfWork.CommitAsync();

            foreach(var p in reqData.Products)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.OrderId = orderCreated.Id;
                orderItem.ProductId = p.ProductId;
                orderItem.Quantity = p.Quantity;
                orderItem.ProductName = p.ProductName ?? "";
                orderItem.TotalPrice = p.TotalPrice;

                await _unitOfWork.OrderItem.AddEntity(orderItem);
                await _unitOfWork.CommitAsync();    
            }

            return orderCreated;

        }   
    }
}
