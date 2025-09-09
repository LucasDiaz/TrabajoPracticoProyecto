using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Models.Request
{
    public class DishRequest
    {
        [Required(ErrorMessage = "El nombre del plato es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre del plato no puede superar los 25 caracteres.")]
        public string Name { get; set; }
       
        public string? Description { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La categoría debe ser un valor válido.")]
        public int Category { get; set; }
        public string? Image { get; set; }




    }
}
