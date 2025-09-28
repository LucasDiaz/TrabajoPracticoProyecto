using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Applications.Models.Request
{
    public class OrderRequest
    {

        public List<ItemRequest> Items { get; set; }    
        public DeliveryRequest Delivery { get; set; }
        public string Notes { get; set; }
    }
}
