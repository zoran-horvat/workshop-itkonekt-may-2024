using System.Diagnostics;
using System.Linq.Expressions;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Invoices;

public abstract class Invoice
{
    protected InvoiceRecord Representation { get; }

    protected Invoice(InvoiceRecord representation) => Representation = representation;

    public Guid Id => this.Representation.Id;
    public Customer Customer => this.Representation.Customer;
    public string Label => this.Representation.Customer.Label;
    public DateOnly IssueDate => DateOnly.FromDateTime(this.Representation.IssueTime);
    public Money Total => this.Representation.Lines
        .Aggregate(Money.Zero, (total, line) => total + line.Price);

    public abstract (string prefix, DateOnly date) Status { get; }

    public InvoiceLine Add(Book book, Money price)
    {
        if (this.Representation.Lines.OfType<BookLine>().FirstOrDefault(line => line.Book.Id == book.Id) is BookLine line)
        {
            line.Increment(1, price);
            return line;
        }

        InvoiceLine newLine = BookLine.CreateNew(this.Representation, book, price);
        this.Representation.Lines.Add(newLine);
        return newLine;
    }
}

public class PaidInvoice : Invoice
{
    internal PaidInvoice(InvoiceRecord representation) : base(representation) { }

    public DateTime PaymentTime => base.Representation.PaymentTime.HasValue
        ? this.Representation.PaymentTime.Value
        : throw new UnreachableException();

    public override (string prefix, DateOnly date) Status => ("Paid", DateOnly.FromDateTime(this.PaymentTime));
}

public abstract class UnpaidInvoice : Invoice
{
    public DateOnly DueDate => base.Representation.DueDate;

    protected UnpaidInvoice(InvoiceRecord representation) : base(representation) { }

    public Invoice Pay(DateTime at)
    {
        base.Representation.PaymentTime = at;
        return new PaidInvoice(base.Representation);
    }
}

public class OpenInvoice : UnpaidInvoice
{
    internal OpenInvoice(InvoiceRecord representation) : base(representation) { }

    public override (string prefix, DateOnly date) Status => ("Due on", base.Representation.DueDate);
}

public class OutstandingInvoice : UnpaidInvoice
{
    internal OutstandingInvoice(InvoiceRecord representation) : base(representation) { }

    public override (string prefix, DateOnly date) Status => ("Past due since", base.Representation.DueDate);
}

public class OverdueInvoice : UnpaidInvoice
{
    internal OverdueInvoice(InvoiceRecord representation) : base(representation) { }

    public override (string prefix, DateOnly date) Status => ("Overdue since", base.Representation.DueDate);
}
