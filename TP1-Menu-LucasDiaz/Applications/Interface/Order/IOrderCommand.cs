using Applications.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Applications.Interface.Order
{
    public interface IOrderCommand
    {

        Task InsertOrder(Domain.Entities.Order order);
        Task UpdateOrder(Domain.Entities.Order order);
        Task RemoveOrder(Domain.Entities.Order order);
    }
}
