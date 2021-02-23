using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactInfo.Application.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        T GetByIdAsync(int id);
        IEnumerable<T> GetAllAsync();
        void CreateAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(int id);
    }
}
