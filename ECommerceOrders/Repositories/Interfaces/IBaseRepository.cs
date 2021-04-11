using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceOrders.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> GetAll();
    }
}