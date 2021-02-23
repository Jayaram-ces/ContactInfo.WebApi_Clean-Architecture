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
        private readonly IUnitOfWork unitOfWork;

        public ContactService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public void CreateAsync(Contact contact)
        {
            unitOfWork.ContactRepository.AddAsync(contact);
        }

        public void DeleteAsync(int id)
        {
            var searchContact = unitOfWork.ContactRepository.GetById(id);

            if (searchContact == null)
                throw new Exception("No such contact found in the database");

            unitOfWork.ContactRepository.DeleteAsync(id);
        }

        public IEnumerable<Contact> GetAllAsync()
        {
            return unitOfWork.ContactRepository.GetAllAsync();
        }

        public Contact GetByIdAsync(int id)
        {
            return unitOfWork.ContactRepository.GetById(id);
        }

        public void UpdateAsync(Contact contact)
        {
            var searchContact = unitOfWork.ContactRepository.GetById(contact.Id);

            if (searchContact == null)
                throw new Exception("No such contact found in the database");

            unitOfWork.ContactRepository.UpdateAsync(contact);
        }
    }
}
