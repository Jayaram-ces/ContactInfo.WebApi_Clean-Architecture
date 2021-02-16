using ContactInfo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfo.Application.Interfaces
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
    }
}
