using Applications.Interface.DeliveryType;
using Applications.Interface.DeliveryType.IDeliveryTypeService;
using Applications.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.DeliveryType
{
    public class DeliveryTypeGetAll: IDeliveryTypeGetAll
    {
        private readonly IDeliveryTypeQuery _query;
        public DeliveryTypeGetAll(IDeliveryTypeQuery deliveryTypeQuery)
        {
            _query = deliveryTypeQuery;
        }
        public async Task<List<DeliveryTypeResponse>> GetAllDeliveryType()
        {
            var deliveryTypes = await _query.GetAllDeliveryTypes();
           return deliveryTypes.Select(dt => new DeliveryTypeResponse
            {
                Id = dt.Id,
                Name = dt.Name
            }).ToList();
           
        }
    }
}
