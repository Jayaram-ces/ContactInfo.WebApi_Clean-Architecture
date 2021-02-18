using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using ContactInfo.Infrastructure.Contexts;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactInfo.Infrastructure.Repository
{
    public class ContactReposistory : IContactRepository
    {
        private readonly IConfiguration _configuration;
        public ContactInfoContext _dataBase;
        public ContactReposistory(IConfiguration configuration, ContactInfoContext dataBase)
        {
            _configuration = configuration;
            _dataBase = dataBase;
        }
        public async Task<int> AddAsync(Contact contact)
        {
            var result = await _dataBase.Contacts.AddAsync(contact);
            return await _dataBase.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var contact = await _dataBase.Contacts.FirstOrDefaultAsync(x => x.Id == id);

            if (contact == null)
                throw new Exception("No such contact found in the database");

            _dataBase.Contacts.Remove(contact);
            return await _dataBase.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Contact>> GetAllAsync()
        {
            return await _dataBase.Contacts.ToListAsync();
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            return await (from c in _dataBase.Contacts
                          where c.Id == id
                          select new Contact
                          {
                              Id = c.Id,
                              FirstName = c.FirstName,
                              LastName = c.LastName,
                              EmailId = c.EmailId,
                              MobileNumber = c.MobileNumber
                          }).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(Contact contact)
        {
            var searchContact = await _dataBase.Contacts.FirstOrDefaultAsync(x => x.Id == contact.Id);

            if (searchContact == null)
                throw new Exception("No such contact found in the database");

            searchContact.EmailId = contact.EmailId;
            searchContact.FirstName = contact.FirstName;
            searchContact.LastName = contact.LastName;
            searchContact.MobileNumber = searchContact.MobileNumber;
            _dataBase.Contacts.Update(searchContact);
            return await _dataBase.SaveChangesAsync();
        }
    }
}
