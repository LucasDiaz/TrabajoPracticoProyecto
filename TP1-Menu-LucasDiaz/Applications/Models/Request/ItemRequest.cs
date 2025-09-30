using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Models.Request
{
    public class ItemRequest
    {
        public Guid Id { get; set; }
        //[Required(ErrorMessage = "La cantidad es requerida .")]
        public int quantity { get; set; }
        public string notes { get; set; }
 

    }
}
