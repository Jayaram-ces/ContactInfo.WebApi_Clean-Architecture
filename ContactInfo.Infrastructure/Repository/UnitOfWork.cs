using ContactInfo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfo.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IContactRepository Contacts { get; }

        public UnitOfWork(IContactRepository contactRepository)
        {
            Contacts = contactRepository;
        }
       
    }
}
