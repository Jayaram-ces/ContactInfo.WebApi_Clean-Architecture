using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactInfo.Application.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<int> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
    }
}
