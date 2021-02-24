using ContactInfo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactInfo.Application.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> GetById(int id);
        Task<IEnumerable<Contact>> GetAllAsync();
        Task AddAsync(Contact entity);
        Task UpdateAsync(Contact entity);
        Task DeleteAsync(int id);
    }
}
