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
        private readonly Mock<IContactService> _contactService = new Mock<IContactService>();
        private readonly ContactController _contactController;

        public ContactControllerTest()
        {
            _contactController = new ContactController(_loggerMock.Object, _contactService.Object);
        }


        #region Get all Contacts 

        [Fact]
        public async Task GetAllAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();

            _contactService.Setup(x => x.GetAllAsync()).Throws(exception);

            var actual = await _contactController.GetAll() as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactService.Verify(m => m.GetAllAsync(), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllAsync_WhenContactReposistorySuccessful_ReturnsOkWithContacts()
        {
            var expected = _fixture.Create<IReadOnlyList<Contact>>();
            _contactService.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

            var actual = await _contactController.GetAll() as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactService.Verify(m => m.GetAllAsync(), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }

        #endregion

        #region Get Contact by Id
        [Fact]
        public async Task GetByIdAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<int>();

            _contactService.Setup(x => x.GetByIdAsync(request)).Throws(exception);

            var actual = await _contactController.GetById(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactService.Verify(m => m.GetByIdAsync(request), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_WhenContactReposistorySuccessful_ReturnsOkWithContact()
        {
            var expected = _fixture.Create<Contact>();
            _contactService.Setup(x => x.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

            var actual = await _contactController.GetById(expected.Id) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactService.Verify(m => m.GetByIdAsync(expected.Id), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_WhenContactReposistoryReturnNull_NotFoundResult()
        {
            string expected = "Not found in the directory.";
            Contact reponse = null;
            var request = _fixture.Create<int>();
            _contactService.Setup(x => x.GetByIdAsync(request)).ReturnsAsync(reponse);

            var actual = await _contactController.GetById(request) as NotFoundObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactService.Verify(m => m.GetByIdAsync(request), Times.Once);
            _contactService.VerifyNoOtherCalls();
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
            _contactService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<Contact>();
            _contactService.Setup(x => x.CreateAsync(request)).Throws(exception);

            var actual = await _contactController.AddAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactService.Verify(m => m.CreateAsync(request), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddAsync_WhenContactReposistorySuccessful_ReturnsOk()
        {
            string expected = "Contact added successfully";
            var request = _fixture.Create<Contact>();

            var actual = await _contactController.AddAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactService.Verify(m => m.CreateAsync(request), Times.Once);
            _contactService.VerifyNoOtherCalls();
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
            _contactService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<Contact>();
            _contactService.Setup(x => x.UpdateAsync(request)).Throws(exception);

            var actual = await _contactController.UpdateAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactService.Verify(m => m.UpdateAsync(request), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_WhenContactReposistorySuccessful_ReturnsOk()
        {
            string expected = "Contact updated successfully";
            var request = _fixture.Create<Contact>();

            var actual = await _contactController.UpdateAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactService.Verify(m => m.UpdateAsync(request), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }
        #endregion

        #region Delete contact
        [Fact]
        public async Task DeleteAsync_WhenContactReposistoryThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<int>();
            _contactService.Setup(x => x.DeleteAsync(request)).Throws(exception);

            var actual = await _contactController.DeleteAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactService.Verify(m => m.DeleteAsync(request), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteContact_WhenContactReposistorySuccessful_ReturnsOk()
        {
            string expected = "Contact Deleted successfully";
            var request = _fixture.Create<int>();

            var actual = await _contactController.DeleteAsync(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactService.Verify(m => m.DeleteAsync(request), Times.Once);
            _contactService.VerifyNoOtherCalls();
        }
        #endregion
    }
}
