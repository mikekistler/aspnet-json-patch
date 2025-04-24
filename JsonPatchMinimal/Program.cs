using Microsoft.EntityFrameworkCore;
using App.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Use Cosmos DB and configure to use the emulator
builder.Services.AddDbContext<AppDb>();

// Create a custom JSON serializer settings
builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Do not serialize null values
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    // Make number handling strict
    options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict;
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Seed the database with mock data
    await SeedData.InitializeDatabaseAsync(app.Services);
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();

app.Run();

public static class SeedData
{
    public static async Task InitializeDatabaseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope(); // Create a scope
        var db = scope.ServiceProvider.GetRequiredService<AppDb>(); // Resolve AppDb within the scope
        await db.Database.EnsureCreatedAsync();
        // // Bail out if the database has already been seeded
        // var exists = await db.Customers.Select(c => c.Id).Take(1).AnyAsync();
        // if (exists)
        // {
        //     return;
        // }
        // // Seed the database with mock data
        // var customer = new Customer
        // {
        //     Id = 1,
        //     CustomerName = "John",
        //     CustomerType = CustomerType.Regular,
        //     Orders = new List<Order>()
        //     {
        //         new Order
        //         {
        //             Id = 1,
        //             OrderName = "Order0"
        //         },
        //         new Order
        //         {
        //             Id = 2,
        //             OrderName = "Order1"
        //         }
        //     }
        // };

        // await db.Customers.AddAsync(customer);
        // await db.SaveChangesAsync();
    }
}