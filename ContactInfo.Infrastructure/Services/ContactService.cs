using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactInfo.Infrastructure.Services
{
    public class ContactService : IContactService
    {
        public readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public Task<int> CreateAsync(Contact contact)
        {
            return  _contactRepository.AddAsync(contact);
        }

        public Task<int> DeleteAsync(int id)
        {
            var searchContact = _contactRepository.GetByIdAsync(id);

            if (searchContact == null)
                throw new Exception("No such contact found in the database");

            return _contactRepository.DeleteAsync(id);
        }

        public Task<IReadOnlyList<Contact>> GetAllAsync()
        {
            return _contactRepository.GetAllAsync();
        }

        public Task<Contact> GetByIdAsync(int id)
        {
            return _contactRepository.GetByIdAsync(id);
        }

        public Task<int> UpdateAsync(Contact contact)
        {
            var searchContact = _contactRepository.GetByIdAsync(contact.Id);

            if (searchContact == null)
                throw new Exception("No such contact found in the database");

            return _contactRepository.UpdateAsync(contact);
        }
    }
}
