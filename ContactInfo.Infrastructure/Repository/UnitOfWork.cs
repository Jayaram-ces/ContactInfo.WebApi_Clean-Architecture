using ContactInfo.Application.Interfaces;

namespace ContactInfo.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IContactService Contacts { get; }

        public UnitOfWork(IContactService contactService)
        {
            Contacts = contactService;
        }     
    }
}
