using ContactInfo.Application.Interfaces;

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
