using Applications.Interface.Dish;
using Applications.Models.Request;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Command
{
    public class DishCommand : IDishCommand
    {
        private readonly AppDbContext _context;
        public DishCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateDish(Dish model)
        {
            _context.Dishes.Add(model);
 
            await _context.SaveChangesAsync();

        }

        public async Task DeleteDish(Guid id)
        {
            var dish = _context.Dishes.FirstOrDefault(x => x.DishId == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDish( Dish model)
        {
            _context.Dishes.Update(model);

            await _context.SaveChangesAsync();
        }
    }
}
