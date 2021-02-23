using ContactInfo.Application.Interfaces;
using MySqlConnector;
using System;
using System.Data;

namespace ContactInfo.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        //public IContactService Contacts { get; }

        //public UnitOfWork(IContactService contactService)
        //{
        //    Contacts = contactService;
        //}     

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IContactRepository _contactRepository;
        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IContactRepository ContactRepository
        {
            get { return _contactRepository ?? (_contactRepository = new ContactReposistory(_transaction)); }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
        }

        private void resetRepositories()
        {
            _contactRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
