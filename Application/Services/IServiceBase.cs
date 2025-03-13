using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IServiceBase<T, TId> where T : class
    {
         Task<IEnumerable<T>> GetAll();
         Task<T?> FindById(int id);
         Task<bool> Create(T entity);
         Task<bool> Edit(T entity, TId id);
         Task<bool> Delete(TId id);
    }
}
