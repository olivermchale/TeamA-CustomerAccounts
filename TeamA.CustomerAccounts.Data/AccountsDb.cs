using Microsoft.EntityFrameworkCore;
using System;
using TeamA.CustomerAccounts.Models;

namespace TeamA.CustomerAccounts.Data
{
    public class AccountsDb : DbContext
    {
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        public AccountsDb(DbContextOptions<AccountsDb> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerAccount>(x =>
            {
                x.Property(c => c.FirstName).IsRequired();
                x.Property(c => c.LastName).IsRequired();
                x.Property(c => c.Email).IsRequired();
                x.Property(c => c.DOB).IsRequired();
                x.Property(c => c.Address).IsRequired();
                x.Property(c => c.Postcode).IsRequired();
                x.Property(c => c.PhoneNumber).IsRequired();
                x.Property(c => c.IsActive).IsRequired();
                x.Property(c => c.IsDeleteRequested).IsRequired();
            });
        }
    }
}
