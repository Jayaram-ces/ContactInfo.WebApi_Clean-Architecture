using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfo.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IContactService Contacts { get; }
    }
}
