using AutoFixture;
using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using ContactInfo.Infrastructure.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ContactInfo.WebApi.Test.ServiceTest
{
    public class ContactServiceTest
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly IContactService _contactService;
        private readonly Mock<IContactRepository> _contactRepository = new Mock<IContactRepository>();

        public ContactServiceTest()
        {
            
            _contactService = new ContactService(_contactRepository.Object);
        }

        [Fact]
        public async void GetAllAsync_GivenValidInput_ShouldReturnAllContacts()
        {
            var expected = _fixture.Build<Contact>().CreateMany().ToList();
            _contactRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

            var actual = await _contactService.GetAllAsync();

            Assert.NotNull(actual);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void GetByIdAsync_GivenValidInput_ShouldReturnContact()
        {
            int random = new Random().Next(0, 2);
            var expected = _fixture.Build<Contact>().CreateMany().ToList();
            _contactRepository.Setup(x => x.GetByIdAsync(random)).ReturnsAsync(expected[random]);

            var actual = await _contactService.GetByIdAsync(random);

            actual.Should().NotBeNull().And.BeEquivalentTo(expected[random]);
        }

        [Fact]
        public async void AddAsync_GivenValidInput_VerifyContactGetsAddedToDB()
        {
            var request = _fixture.Build<Contact>().With(i => i.Id, 44444).Create();
            _contactRepository.Setup(x => x.AddAsync(request)).ReturnsAsync(1);

            var actual = await _contactService.CreateAsync(request);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void DeleteAsync_GivenValidInput_VerifyContactDeletedFromDB()
        {
            var request = _fixture.Create<int>();
            _contactRepository.Setup(x => x.DeleteAsync(request)).ReturnsAsync(1);

            var actual = await _contactService.DeleteAsync(request);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void UpdateAsync_GivenInValidInput_VerifyContactUpdatedInDB()
        {
            var request = _fixture.Create<Contact>();
            _contactRepository.Setup(x => x.UpdateAsync(request)).ReturnsAsync(1);

            var actual = await _contactService.UpdateAsync(request);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void DeleteAsync_GivenInValidInput_VerifyDeleteAsyncThrowsException()
        {
            var request = _fixture.Create<int>();
            Mock<Exception> exp = new Mock<Exception>();
            exp.Setup(x => x.Message).Returns("No such contact found in the database");
            _contactRepository.Setup(x => x.DeleteAsync(request)).ThrowsAsync(exp.Object);

            Assert.ThrowsAnyAsync<Exception>(() => _contactService.DeleteAsync(request)).Result.Message.Should().BeEquivalentTo("No such contact found in the database");
        }

        [Fact]
        public async void UpdateAsync_GivenInValidInput_VerifyUpdateAsyncThrowsException()
        {
            var request = _fixture.Create<Contact>();

            Mock<Exception> exp = new Mock<Exception>();
            exp.Setup(x => x.Message).Returns("No such contact found in the database");
            _contactRepository.Setup(x => x.UpdateAsync(request)).ThrowsAsync(exp.Object);


            Assert.ThrowsAnyAsync<Exception>(() => _contactService.UpdateAsync(request)).Result.Message.Should().BeEquivalentTo("No such contact found in the database");
        }
    }
}
