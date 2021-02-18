using AutoFixture;
using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using ContactInfo.Infrastructure.Contexts;
using ContactInfo.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Xunit;

namespace ContactInfo.WebApi.Test.ReposistoryTest
{
    public class ContactReposistoryTest
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly ContactInfoContext _dbContext;
        private readonly IContactRepository _contactRepository;
        private readonly IConfiguration _configuration;

        public ContactReposistoryTest()
        {
            _dbContext = new ContactInfoContext(new DbContextOptionsBuilder<ContactInfoContext>()
                .UseInMemoryDatabase(databaseName: "ContactDB")
                .Options);
            _contactRepository = new ContactReposistory(_configuration, _dbContext);
        }

        [Fact]
        public async void GetAllAsync_GivenValidInput_ShouldReturnAllContacts()
        {
            var expected = _fixture.Build<Contact>().CreateMany().ToList();
            await _dbContext.AddRangeAsync(expected);
            await _dbContext.SaveChangesAsync();

            var actual = await _contactRepository.GetAllAsync();

            Assert.NotNull(actual);
            actual.Should().Contain(expected);
        }

        [Fact]
        public async void GetByIdAsync_GivenValidInput_ShouldReturnContact()
        {
            int random = new Random().Next(0, 2);
            var expected = _fixture.Build<Contact>().CreateMany().ToList();
            await _dbContext.AddRangeAsync(expected);
            await _dbContext.SaveChangesAsync();

            var actual = await _contactRepository.GetByIdAsync(expected[random].Id);

            actual.Should().NotBeNull().And.BeEquivalentTo(expected[random]);
        }

        [Fact]
        public async void AddAsync_GivenValidInput_VerifyContactGetsAddedToDB()
        {
            var expected = _fixture.Build<Contact>().With(i => i.Id, 44444).Create();

            await _contactRepository.AddAsync(expected);

            var actual = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == expected.Id);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async void DeleteAsync_GivenValidInput_VerifyContactDeletedFromDB()
        {
            var expected = _fixture.Build<Contact>().With(i => i.Id, 33333).Create();
            await _dbContext.AddRangeAsync(expected);
            await _dbContext.SaveChangesAsync();

            await _contactRepository.DeleteAsync(expected.Id);

            var actual = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == expected.Id);
            actual.Should().BeNull().And.NotBeSameAs(expected);
        }

        [Fact]
        public async void UpdateAsync_GivenInValidInput_VerifyContactUpdatedInDB()
        {
            string updateFirstName = _fixture.Create<string>();
            var expected = _fixture.Build<Contact>().With(i => i.Id, 5555).Create();
            await _dbContext.AddRangeAsync(expected);
            await _dbContext.SaveChangesAsync();
            expected.FirstName = updateFirstName;

            await _contactRepository.UpdateAsync(expected);

            var actual = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == expected.Id);
            actual.FirstName.Should().NotBeNull().And.BeEquivalentTo(expected.FirstName);
        }

        [Fact]
        public async void DeleteAsync_GivenInValidInput_VerifyDeleteAsyncThrowsException()
        {

            var input = _fixture.Build<Contact>().With(x => x.Id, 22222).Create();

            Assert.ThrowsAsync<Exception>(() => _contactRepository.DeleteAsync(input.Id)).Result.Message.Should().BeEquivalentTo("No such contact found in the database");
        }

        [Fact]
        public async void UpdateAsync_GivenInValidInput_VerifyUpdateAsyncThrowsException()
        {
            var input = _fixture.Build<Contact>().With(x => x.Id, 11111).Create();

            Assert.ThrowsAsync<Exception>(() => _contactRepository.UpdateAsync(input)).Result.Message.Should().BeEquivalentTo("No such contact found in the database");
        }
    }
}
