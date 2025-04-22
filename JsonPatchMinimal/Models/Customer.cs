namespace App.Models;

public class Customer
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public List<Order>? Orders { get; set; }
}
