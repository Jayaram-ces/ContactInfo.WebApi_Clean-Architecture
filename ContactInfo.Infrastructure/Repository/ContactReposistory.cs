using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfo.Infrastructure.Repository
{
    internal class ContactReposistory : RepositoryBase, IContactRepository
    {
        public ContactReposistory(IDbTransaction transaction) : base(transaction)
        {
        }
        public async Task AddAsync(Contact contact)
        {
            Connection.Execute(
                "Insert into Contacts (Id,FirstName,LastName,MobileNumber,EmailId) VALUES (@Id,@FirstName,@LastName,@MobileNumber,@EmailId)",
                param: new { Id = contact.Id, FirstName = contact.FirstName, LastName = contact.LastName, MobileNumber = contact.MobileNumber, EmailId = contact.EmailId },
                transaction: Transaction
            );

            Transaction.Commit();
        }

        public async Task DeleteAsync(int id)
        {
            var result = Connection.Execute(
                "DELETE FROM Contacts WHERE Id = @Id",
                param: new { Id = id },
                transaction: Transaction
            );

            Transaction.Commit();

        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return Connection.Query<Contact>("SELECT * FROM Contacts",
                                                 transaction: Transaction
                                            ).ToList();
        }

        public async Task<Contact> GetById(int id)
        {
            return Connection.Query<Contact>("SELECT * FROM Contacts WHERE Id = @Id",
                                                  param: new { Id = id },
                                                  transaction: Transaction
                                            ).FirstOrDefault();
        }

        public async Task UpdateAsync(Contact contact)
        {
            Connection.Execute(
                "UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, MobileNumber = @MobileNumber, EmailId = @EmailId  WHERE Id = @Id",
                param: new { Id = contact.Id, FirstName = contact.FirstName, LastName = contact.LastName, MobileNumber = contact.MobileNumber, EmailId = contact.EmailId },
                transaction: Transaction
            );

            Transaction.Commit();

        }
    }
}
