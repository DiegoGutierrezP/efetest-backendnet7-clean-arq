﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
    
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
