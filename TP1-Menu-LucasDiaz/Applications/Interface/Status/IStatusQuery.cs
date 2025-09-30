using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Status
{
    public interface IStatusQuery
    {
        Task<string> GetStatusById(int id);
        Task<List<Domain.Entities.Status>> GetAllStatuses();
        Task<bool> StatusExist(int id);
    }
}
