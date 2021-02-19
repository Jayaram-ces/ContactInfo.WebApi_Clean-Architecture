using ContactInfo.Application.Interfaces;
using ContactInfo.Infrastructure.Contexts;
using ContactInfo.Infrastructure.Repository;
using ContactInfo.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfo.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IContactRepository, ContactReposistory>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            string _defaultConnection = configuration.GetConnectionString("DefaultConnection");
            string _identityConnection = configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(_defaultConnection, ServerVersion.AutoDetect(_defaultConnection)));
            services.AddDbContext<ContactInfoContext>(options => options.UseMySql(_identityConnection, ServerVersion.AutoDetect(_identityConnection)));
        }
    }
}
