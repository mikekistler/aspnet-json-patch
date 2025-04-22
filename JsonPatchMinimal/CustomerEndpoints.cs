using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Http.HttpResults;

internal static class CustomerEndpoints {
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/customers").WithTags("Customers");

        group.MapGet("/", async Task<Ok<Customer[]>> (AppDb db) =>
        {
            return TypedResults.Ok(await db.Customers.Include(c => c.Orders).ToArrayAsync());
        })
        .WithName("ListCustomers");

        group.MapGet("/{id}", async Task<Results<Ok<Customer>,NotFound>> (AppDb db, int id) =>
        {
            return await db.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id) is Customer customer
                ? TypedResults.Ok(customer)
                : TypedResults.NotFound();
        })
        .WithName("GetCustomer");

        group.MapPost("/", async (AppDb db, Customer customer) =>
        {
            await db.Customers.AddAsync(customer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/customers/{customer.Id}", customer);
        })
        .WithName("CreateCustomer");

        group.MapDelete("/{id}", async Task<Results<NoContent,NotFound>> (AppDb db, int id) =>
        {
            if (await db.Customers.FindAsync(id) is Customer customer)
            {
                db.Customers.Remove(customer);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        })
        .WithName("DeleteCustomer");
    }
}