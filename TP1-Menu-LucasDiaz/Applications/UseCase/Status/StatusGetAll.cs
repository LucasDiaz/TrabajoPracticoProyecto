using Applications.Interface.Status;
using Applications.Interface.Status.IStatusService;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.Status
{
    public class StatusGetAll: IStatusGetAll
    {
        private readonly IStatusQuery _query;
        public StatusGetAll(IStatusQuery statusQuery)
        {
            _query = statusQuery;
        }
        public async Task<List<StatusResponse>> GetAllStatuses()
        {
            var statuses = await _query.GetAllStatuses();
            return statuses.Select(s => new StatusResponse
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}
