using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Models.Response
{
    public class OrderItemResponse
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public string notes { get; set; }
        public GenericResponse status { get; set; }
        public DishShortResponse dish { get; set; }
    }
}
