using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Models.Request
{
    public class DeliveryRequest
    {
        //[Required(ErrorMessage = "La Id del delivery es requerido .")]
        public int id { get; set; }
        
        //[StringLength(255, ErrorMessage = "El nombre del plato no puede superar los 255 caracteres.")]
        public string to { get; set; }   
    }
}
