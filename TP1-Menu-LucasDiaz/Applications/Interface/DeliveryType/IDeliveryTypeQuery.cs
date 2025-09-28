using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.DeliveryType
{
    public interface IDeliveryTypeQuery
    {
        Task<List<Domain.Entities.DeliveryType>> GetAllDeliveryTypes();
        Task<Domain.Entities.DeliveryType?> GetDeliveryTypeById(int id);
    }
}
