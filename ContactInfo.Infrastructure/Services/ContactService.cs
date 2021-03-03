using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactInfo.Infrastructure.Services
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork unitOfWork;

        public ContactService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public async Task CreateAsync(Contact contact)
        {
            await unitOfWork.ContactRepository.AddAsync(contact);
        }

        public async Task DeleteAsync(int id)
        {
            var searchContact = await unitOfWork.ContactRepository.GetById(id);

            if (searchContact == null)
                throw new Exception("No such contact found in the database");

            await unitOfWork.ContactRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await unitOfWork.ContactRepository.GetAllAsync();
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            return await unitOfWork.ContactRepository.GetById(id);
        }

        public async Task UpdateAsync(Contact contact)
        {
            var searchContact = await unitOfWork.ContactRepository.GetById(contact.Id);

            if (searchContact == null)
                throw new Exception("No such contact found in the database");

            await unitOfWork.ContactRepository.UpdateAsync(contact);
        }

        public async Task<Contact> PatchAsync(int id, JsonPatchDocument<Contact> entity)
        {
            var searchContact = await unitOfWork.ContactRepository.GetById(id);

            if (searchContact == null)
                throw new Exception("No such contact found in the database");

            return await unitOfWork.ContactRepository.PatchAsync(searchContact, entity);
        }
    }
}
