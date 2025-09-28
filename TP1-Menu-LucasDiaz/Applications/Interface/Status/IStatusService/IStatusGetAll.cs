using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Status.IStatusService
{
    public interface IStatusGetAll
    {
        Task<List<StatusResponse>> GetAllStatuses();
    }
}
