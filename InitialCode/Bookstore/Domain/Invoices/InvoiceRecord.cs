using Bookstore.Domain.Common;
using Bookstore.Domain.Invoices;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Invoices;

public class InvoiceRecord
{
    public Guid Id { get; internal set; } = Guid.Empty;
    public Customer Customer { get; internal set; } = null!;
    public DateTime IssueTime { get; internal set; }
    public DateOnly DueDate { get; internal set; }
    public DateTime? PaymentTime { get; internal set; }
    internal List<InvoiceLine> Lines { get; } = new List<InvoiceLine>();

    public Money TotalAmount =>
        this.Lines.Aggregate(Money.Zero, (total, line) => total + line.Price);

    private InvoiceRecord() { }      // Used by EF Core

    public InvoiceRecord(Guid id, Customer customer, DateTime issueTime, DateOnly dueDate)
    {
        Id = id;
        Customer = customer;
        IssueTime = issueTime;
        DueDate = dueDate;
    }
}
