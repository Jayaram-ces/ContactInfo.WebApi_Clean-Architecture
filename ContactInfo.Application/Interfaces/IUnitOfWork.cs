using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfo.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IContactService Contacts { get; }
        IContactRepository ContactRepository { get; }

        void Commit();
    }
}
