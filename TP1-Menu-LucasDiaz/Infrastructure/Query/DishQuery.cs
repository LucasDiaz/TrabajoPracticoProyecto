using Applications.Enum;
using Applications.Interface.Dish;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class DishQuery : IDishQuery
    {
        private readonly AppDbContext _context;
        public DishQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Dish>> GetAllAsync(string? name = null, int? categoryId = null, OrderPrice? priceOrder = OrderPrice.ASC)
        {
            var query = _context.Dishes.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(d => d.CategoryId == categoryId.Value);
            }

            switch (priceOrder)
            {
                case OrderPrice.ASC:
                    query = query.OrderBy(d => d.Price);
                    break;
                case OrderPrice.DESC:
                    query = query.OrderByDescending(d => d.Price);
                    break;
                default:
                    throw new InvalidOperationException("Valor de ordenamiento inválido");

            }
            return await query
                    .Include(d => d.Category)
                    .ToListAsync();


        }

        public async Task<IEnumerable<Dish>> GetAllDishes()
        {
            return await _context.Dishes.ToListAsync();
        }

        public async Task<Dish?> GetDishById(Guid id)
        {
            return await _context.Dishes
                                .Include(d => d.Category) // <--- ¡Esto es lo que faltaba!
                                .FirstOrDefaultAsync(d => d.DishId == id);
            //return await _context.Dishes.FindAsync(id).AsTask();
        }
        public async Task<bool> GetDishByName(string name)
        {
            return await _context.Dishes.AnyAsync(d => d.Name == name);
        }

        public async Task<bool> DishExistsById(Guid id)
        {
            return await _context.Dishes.AnyAsync(d => d.DishId == id);
        }

        public async Task<bool> DishExists(string name, Guid? id)
        {
            var query = _context.Dishes.AsQueryable();

            if (id.HasValue)
            {
                // Si estamos actualizando, excluimos el ID actual de la búsqueda
                query = query.Where(d => d.DishId != id.Value);
            }

            // Ahora la búsqueda de conflicto solo se hará en los OTROS platos
            return await query.AnyAsync(d => d.Name == name);

        }

    }
}

        