using System.Text.Json.Serialization;

namespace App.Models;

public class Customer
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public CustomerType CustomerType { get; set; }
    public List<Order>? Orders { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<CustomerType>))]
public enum CustomerType
{
    Regular,
    Premium,
    VIP
}