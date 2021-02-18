using AutoFixture;
using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using ContactInfo.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ContactInfo.WebApi.Test.ControllerTest
{
    public class ContactControllerTest
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ILogger<ContactController>> _loggerMock = new Mock<ILogger<ContactController>>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly ContactController _contactController;

        public ContactControllerTest()
        {
            _contactController = new ContactController(_unitOfWorkMock.Object, _loggerMock.Object);
        }


        #region Get all Contacts 

        [Fact]
        public async Task GetAllAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();

            _unitOfWorkMock.Setup(x => x.Contacts.GetAllAsync()).ThrowsAsync(exception);

            var actual = await _contactController.GetAll() as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.GetAllAsync(), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void GetAllAsync_WhenContactReposistorySuccessful_ReturnsOkWithContacts()
        {
            var expected = _fixture.Create<IReadOnlyList<Contact>>();
            _unitOfWorkMock.Setup(x => x.Contacts.GetAllAsync()).ReturnsAsync(expected);

            var actual = await _contactController.GetAll() as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.GetAllAsync(), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        #endregion

        #region Get Contact by Id
        [Fact]
        public async Task GetByIdAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<int>();

            _unitOfWorkMock.Setup(x => x.Contacts.GetByIdAsync(request)).ThrowsAsync(exception);

            var actual = await _contactController.GetById(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.GetByIdAsync(request), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_WhenContactReposistorySuccessful_ReturnsOkWithContact()
        {
            var expected = _fixture.Create<Contact>();
            _unitOfWorkMock.Setup(x => x.Contacts.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

            var actual = await _contactController.GetById(expected.Id) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.GetByIdAsync(expected.Id), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_WhenContactReposistoryReturnNull_NotFoundResult()
        {
            string expected = "Not found in the directory.";
            Contact reponse = null;
            var request = _fixture.Create<int>();
            _unitOfWorkMock.Setup(x => x.Contacts.GetByIdAsync(request)).ReturnsAsync(reponse);

            var actual = await _contactController.GetById(request) as NotFoundObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.GetByIdAsync(request), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Add Contact

        [Fact]
        public async Task AddAsync_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            var request = _fixture.Build<Contact>().Without(e => e.EmailId).Create();
            _contactController.ModelState.AddModelError("EmailId", "Required");

            var actual = await _contactController.AddAsync(request) as BadRequestObjectResult;

            Assert.NotNull(actual);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<Contact>();
            _unitOfWorkMock.Setup(x => x.Contacts.AddAsync(request)).ThrowsAsync(exception);

            var actual = await _contactController.AddAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.AddAsync(request), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void AddAsync_WhenContactReposistorySuccessful_ReturnsOk()
        {
            string expected = "Contact added successfully";
            var request = _fixture.Create<Contact>();
            _unitOfWorkMock.Setup(x => x.Contacts.AddAsync(request)).ReturnsAsync(1);

            var actual = await _contactController.AddAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.AddAsync(request), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        #endregion

        #region Update contact
        [Fact]
        public async Task UpdateAsync_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            var request = _fixture.Build<Contact>().Without(e => e.EmailId).Create();
            _contactController.ModelState.AddModelError("EmailId", "Required");

            var actual = await _contactController.UpdateAsync(request) as BadRequestObjectResult;

            Assert.NotNull(actual);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<Contact>();
            _unitOfWorkMock.Setup(x => x.Contacts.UpdateAsync(request)).ThrowsAsync(exception);

            var actual = await _contactController.UpdateAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.UpdateAsync(request), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UpdateAsync_WhenContactReposistorySuccessful_ReturnsOk()
        {
            string expected = "Contact updated successfully";
            var request = _fixture.Create<Contact>();
            _unitOfWorkMock.Setup(x => x.Contacts.UpdateAsync(request)).ReturnsAsync(1);

            var actual = await _contactController.UpdateAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.UpdateAsync(request), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Delete contact
        [Fact]
        public async Task DeleteAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<int>();
            _unitOfWorkMock.Setup(x => x.Contacts.DeleteAsync(request)).ThrowsAsync(exception);

            var actual = await _contactController.DeleteAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.DeleteAsync(request), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void DeleteContact_WhenContactReposistorySuccessful_ReturnsOk()
        {
            string expected = "Contact Deleted successfully";
            var request = _fixture.Create<int>();
            _unitOfWorkMock.Setup(x => x.Contacts.DeleteAsync(request)).ReturnsAsync(1);

            var actual = await _contactController.DeleteAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _unitOfWorkMock.Verify(m => m.Contacts.DeleteAsync(request), Times.Once);
            _unitOfWorkMock.VerifyNoOtherCalls();
        }
        #endregion
    }
}
