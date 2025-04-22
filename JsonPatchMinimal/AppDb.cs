using Microsoft.EntityFrameworkCore;
using App.Models;

class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options)
        : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
}