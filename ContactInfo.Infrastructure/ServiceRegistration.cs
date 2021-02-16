using ContactInfo.Application.Interfaces;
using ContactInfo.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfo.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IContactRepository, ContactReposistory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
