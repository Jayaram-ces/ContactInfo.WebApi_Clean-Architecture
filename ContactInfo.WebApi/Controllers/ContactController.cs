using ContactInfo.Application.Interfaces;
using ContactInfo.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactInfo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ContactController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await unitOfWork.Contacts.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await unitOfWork.Contacts.GetByIdAsync(id);
            if (data == null) return Ok();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Contact contact)
        {
            var data = await unitOfWork.Contacts.AddAsync(contact);
            return Ok(data);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.Contacts.DeleteAsync(id);
            return Ok(data);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Contact contact)
        {
            var data = await unitOfWork.Contacts.UpdateAsync(contact);
            return Ok(data);
        }
    }
}
