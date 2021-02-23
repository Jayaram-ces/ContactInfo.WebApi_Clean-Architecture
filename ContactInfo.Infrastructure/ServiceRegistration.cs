﻿using ContactInfo.Application.Interfaces;
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
            services.AddScoped<IUnitOfWork, UnitOfWork>(provider => new UnitOfWork(configuration.GetConnectionString("IdentityConnection")));
            services.AddTransient<IContactService, ContactService>();

            string _defaultConnection = configuration.GetConnectionString("DefaultConnection");
            string _identityConnection = configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(_defaultConnection, ServerVersion.AutoDetect(_defaultConnection)));
        }
    }
}
