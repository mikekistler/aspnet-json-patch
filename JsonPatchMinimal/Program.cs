using Microsoft.EntityFrameworkCore;
using App.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDb>(opt => opt.UseInMemoryDatabase("AppDb"));

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
    app.InitializeDatabase();
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();

app.Run();

public static class SeedData
{
    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
        var serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
        var db = serviceProvider.GetRequiredService<AppDb>();
        db.Database.EnsureCreated();
        // Bail out if the database has already been seeded
        if (db.Customers.Any())
        {
            return app;
        }
        // Seed the database with mock data
        var customer = new Customer
        {
            Id = 1,
            CustomerName = "John",
            CustomerType = CustomerType.Regular,
            Orders = new List<Order>()
            {
                new Order
                {
                    Id = 1,
                    OrderName = "Order0"
                },
                new Order
                {
                    Id = 2,
                    OrderName = "Order1"
                }
            }
        };

        db.Customers.Add(customer);
        db.SaveChanges();

        return app;
    }
}