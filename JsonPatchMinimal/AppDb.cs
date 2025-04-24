using Microsoft.EntityFrameworkCore;
using App.Models;

public class AppDb : DbContext
{
    public required DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var accountKey = new ConfigurationBuilder()
            .AddUserSecrets<AppDb>()
            .Build()
            .GetSection("CosmosDb:AccountKey")
            .Value;

        optionsBuilder.UseCosmos(
            accountEndpoint: "https://mdkdemo.documents.azure.com:443/",
            accountKey: accountKey!,
            databaseName: "AppDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .ToContainer("Customers") // Maps to the "Customers" container
            .HasPartitionKey(c => c.Id); // Use Id as the partition key
    }
}