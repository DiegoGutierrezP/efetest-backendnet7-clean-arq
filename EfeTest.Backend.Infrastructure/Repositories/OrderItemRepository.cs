﻿using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Domain.Entities;
using EfeTest.Backend.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Infrastructure.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
