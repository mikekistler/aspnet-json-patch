using Microsoft.AspNetCore.Mvc;

using App.Data;
using App.Models;

namespace App.Controllers;

[ApiController]
[Route("/api/customers")]
public class CustomerController : ControllerBase
{
    [HttpGet("{id}", Name = "GetCustomer")]
    public Customer Get(AppDb db, string id)
    {
        // Retrieve the customer by ID
        var customer = db.Customers.FirstOrDefault(c => c.Id == id);

        // Return 404 Not Found if customer doesn't exist
        if (customer == null)
        {
            Response.StatusCode = 404;
            return null;
        }

        return customer;
    }
}
