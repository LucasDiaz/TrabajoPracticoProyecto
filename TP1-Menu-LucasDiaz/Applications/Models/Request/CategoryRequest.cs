using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Models.Request
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(25, ErrorMessage = "El nombre de la categoría no puede superar los 25 caracteres.")]
        public string Name { get; set; }
        [StringLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo de orden es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El orden debe ser un número positivo.")]
        public int Order { get; set; }

    }
}
