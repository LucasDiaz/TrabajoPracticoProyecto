using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Models.Request
{
    public class OrderRequest
    {
     
        public string DeliveryTo { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        

        public int StatusId { get; set; }

        public int DeliveryTypeId { get; set; }
    }
}
