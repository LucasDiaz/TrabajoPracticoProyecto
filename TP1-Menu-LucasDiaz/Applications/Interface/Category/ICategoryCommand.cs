using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Category
{
    public interface ICategoryCommand
    {
        Task InsertCategory(Domain.Entities.Category category);
        Task UpdateCategory(Domain.Entities.Category category);
        Task RemoveCategory(Domain.Entities.Category category);
    }
}
