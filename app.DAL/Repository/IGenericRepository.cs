using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Repository
{
    public interface IGenericRepository<T> where T: class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task<T> PostAsync(T entity);
        Task<List<T>> PostMultiple(List<T> entities);
        Task<T> DeleteAsync(Guid id);
        Task<T> UpdateAsync(Guid id, T entity);
        Task<List<T>> DeleteAllAsync();

    }
}
