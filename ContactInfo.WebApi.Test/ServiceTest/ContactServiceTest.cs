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
        private readonly Mock<IUnitOfWork> _uow = new Mock<IUnitOfWork>();

        public ContactServiceTest()
        {
            
            _contactService = new ContactService(_uow.Object);
        }

        [Fact]
        public async void GetAllAsync_GivenValidInput_ShouldReturnAllContacts()
        {
            var expected = _fixture.Build<Contact>().CreateMany().ToList();
            _uow.Setup(x => x.ContactRepository.GetAllAsync()).Returns(expected);

            var actual = _contactService.GetAllAsync();

            Assert.NotNull(actual);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void GetByIdAsync_GivenValidInput_ShouldReturnContact()
        {
            int random = new Random().Next(0, 2);
            var expected = _fixture.Build<Contact>().CreateMany().ToList();
            _uow.Setup(x => x.ContactRepository.GetById(random)).Returns(expected[random]);

            var actual = _contactService.GetByIdAsync(random);

            actual.Should().NotBeNull().And.BeEquivalentTo(expected[random]);
        }

        [Fact]
        public async void AddAsync_GivenValidInput_VerifyContactGetsAddedToDB()
        {
            var request = _fixture.Build<Contact>().With(i => i.Id, 44444).Create();
            _uow.Setup(x => x.ContactRepository.AddAsync(request));

            _contactService.CreateAsync(request);

            _uow.Verify(x => x.ContactRepository.AddAsync(request), Times.Once);
        }

        [Fact]
        public void DeleteAsync_GivenValidInput_VerifyContactDeletedFromDB()
        {
            var request = _fixture.Create<int>();
            var response = _fixture.Build<Contact>().With(i => i.Id, 55555).Create();
            _uow.Setup(x => x.ContactRepository.GetById(request)).Returns(response);
            _uow.Setup(x => x.ContactRepository.DeleteAsync(request));

            _contactService.DeleteAsync(request);

            _uow.Verify(x => x.ContactRepository.DeleteAsync(request), Times.Once);
        }

        [Fact]
        public async void UpdateAsync_GivenInValidInput_VerifyContactUpdatedInDB()
        {
            var request = _fixture.Create<Contact>();
            var response = _fixture.Build<Contact>().With(i => i.Id, 66666).Create();
            _uow.Setup(x => x.ContactRepository.GetById(request.Id)).Returns(response);
            _uow.Setup(x => x.ContactRepository.UpdateAsync(request));

            _contactService.UpdateAsync(request);

            _uow.Verify(x => x.ContactRepository.UpdateAsync(request), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_GivenInValidInput_VerifyDeleteAsyncThrowsException()
        {
            var request = _fixture.Create<int>();
            Mock<Exception> exp = new Mock<Exception>();
            exp.Setup(x => x.Message).Returns("No such contact found in the database");
            _uow.Setup(x => x.ContactRepository.DeleteAsync(request)).Throws(exp.Object);

            Assert.Throws<Exception>(() => _contactService.DeleteAsync(request)).Message.Should().BeEquivalentTo("No such contact found in the database");
        }

        [Fact]
        public async void UpdateAsync_GivenInValidInput_VerifyUpdateAsyncThrowsException()
        {
            var request = _fixture.Create<Contact>();

            Mock<Exception> exp = new Mock<Exception>();
            exp.Setup(x => x.Message).Returns("No such contact found in the database");
            _uow.Setup(x => x.ContactRepository.UpdateAsync(request)).Throws(exp.Object);


            Assert.Throws<Exception>(() => _contactService.UpdateAsync(request)).Message.Should().BeEquivalentTo("No such contact found in the database");
        }
    }
}
