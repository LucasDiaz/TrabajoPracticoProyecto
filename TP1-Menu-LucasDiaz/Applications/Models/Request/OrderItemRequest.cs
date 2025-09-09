using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Models.Request
{
    public class OrderItemRequest
    {
      
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public DateTime CreateDate { get; set; }

        public Guid DishId { get; set; }

        public long OrderId { get; set; }

        public int StatusId { get; set; }
    }
}
