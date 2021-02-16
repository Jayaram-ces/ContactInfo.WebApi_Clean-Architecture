using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using Dapper;
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
        private readonly IConfiguration configuration;
        public ContactReposistory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<int> AddAsync(Contact entity)
        {
            entity.AddedOn = DateTime.Now;
            var sql = "Insert into Contacts (FirstName,LastName,MobileNumber,EmailId,AddedOn) VALUES (@FirstName,@LastName,@MobileNumber,@EmailId,@AddedOn)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Contacts WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<Contact>> GetAllAsync()
        {
            var sql = "SELECT * FROM Contacts";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Contact>(sql);
                return result.ToList();
            }
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Contacts WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Contact>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<int> UpdateAsync(Contact entity)
        {
            entity.ModifiedOn = DateTime.Now;
            var sql = "UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, MobileNumber = @MobileNumber, EmailId = @EmailId, ModifiedOn = @ModifiedOn  WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }
    }
}
