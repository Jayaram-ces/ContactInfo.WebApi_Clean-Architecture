using ContactInfo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfo.Infrastructure.Contexts
{
    public partial class ContactInfoContext : DbContext
    {
        public ContactInfoContext(DbContextOptions<ContactInfoContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=JayCes251120$;database=ContactIdentityDB", ServerVersion.FromString("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region MySql code first approach
            //Run below command in Package Manager Console to update database.
            //update - database - Context ContactInfoContext
            //update - database - Context ApplicationDbContext

            // Map entities to tables
            modelBuilder.Entity<Contact>().ToTable("Contacts");

            // Configure Primary Keys
            modelBuilder.Entity<Contact>().HasKey(ug => ug.Id).HasName("CES_Contacts");

            // Configure columns
            modelBuilder.Entity<Contact>().Property(ug => ug.Id).HasColumnType("int").UseIdentityColumn().IsRequired();
            modelBuilder.Entity<Contact>().Property(ug => ug.FirstName).HasColumnType("varchar(45)").IsRequired();
            modelBuilder.Entity<Contact>().Property(ug => ug.LastName).HasColumnType("varchar(45)").IsRequired();
            modelBuilder.Entity<Contact>().Property(ug => ug.MobileNumber).HasColumnType("varchar(45)").IsRequired(false);
            modelBuilder.Entity<Contact>().Property(ug => ug.EmailId).HasColumnType("varchar(45)").IsRequired(true);

            #endregion
        }
    }
}
