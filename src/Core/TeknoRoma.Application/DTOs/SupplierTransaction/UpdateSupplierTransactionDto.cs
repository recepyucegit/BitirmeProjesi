namespace TeknoRoma.Application.DTOs.SupplierTransaction;

public class UpdateSupplierTransactionDto
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public int? ProductId { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? ReferenceNumber { get; set; }
    public string Status { get; set; } = string.Empty;
}
