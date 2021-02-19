using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfo.Infrastructure.Repository
{
    public class ContactReposistory : IContactRepository
    {
        private readonly IConfiguration _configuration;
        public ContactReposistory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<int> AddAsync(Contact contact)
        {
            var sql = "Insert into Contacts (Id,FirstName,LastName,MobileNumber,EmailId) VALUES (@Id,@FirstName,@LastName,@MobileNumber,@EmailId)";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("IdentityConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, contact);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Contacts WHERE Id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("IdentityConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<Contact>> GetAllAsync()
        {
            var sql = "SELECT * FROM Contacts";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("IdentityConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Contact>(sql);
                return result.ToList();
            }
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Contacts WHERE Id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("IdentityConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Contact>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<int> UpdateAsync(Contact contact)
        {
            var sql = "UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, MobileNumber = @MobileNumber, EmailId = @EmailId  WHERE Id = @Id";
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("IdentityConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, contact);
                return result;
            }
        }
    }
}
