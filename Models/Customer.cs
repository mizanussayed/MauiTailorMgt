using System.ComponentModel.DataAnnotations;

namespace MYPM.Models;

public sealed class Customer
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Mobile { get; set; } = string.Empty;
    public string? Address { get; set; }
}

public class Invoice
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public Customer Customer { get; set; } = new Customer();
    public List<InvoiceItem> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
}

public class InvoiceItem
{
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
