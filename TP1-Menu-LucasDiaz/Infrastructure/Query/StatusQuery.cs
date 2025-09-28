using Applications.Interface.Status;
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
    public class StatusQuery : IStatusQuery
    {
        private readonly AppDbContext _context;
        public StatusQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetStatusById(int id)
        {
            var status = await _context.Statuses.FindAsync(id).AsTask();
            return status.Name;
        }

        public async Task<List<Status>> GetAllStatuses()
        {
            return await _context.Statuses.ToListAsync();
        }
    }
}
