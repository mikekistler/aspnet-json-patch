using System.Text.Json.Serialization;

namespace App.Models;

public class CustomerPoco
{
    public string? CustomerName { get; set; }
    public CustomerType CustomerType { get; set; }
    public List<Order>? Orders { get; set; }
}

public class Customer : CustomerPoco
{
    public int Id { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<CustomerType>))]
public enum CustomerType
{
    Regular,
    Premium,
    VIP
}