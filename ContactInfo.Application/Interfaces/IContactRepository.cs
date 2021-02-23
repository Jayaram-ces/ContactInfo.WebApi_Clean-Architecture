using ContactInfo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactInfo.Application.Interfaces
{
    public interface IContactRepository
    {
        Contact GetById(int id);
        IEnumerable<Contact> GetAllAsync();
        void AddAsync(Contact entity);
        void UpdateAsync(Contact entity);
        void DeleteAsync(int id);
    }
}
