using Bookstore.Domain.Common;
using Bookstore.Domain.Invoices;

namespace Bookstore.Domain.Models;

public class BookLine : InvoiceLine
{
    public Book Book { get; private set; } = null!;

    public BookLine() { }      // Used by EF Core

    public BookLine(Guid invoiceId, Guid id, string label, int quantity, Money price) : base(invoiceId, id, label, quantity, price) { }

    public static BookLine CreateNew(InvoiceRecord invoice, Book book, Money price) =>
        new(invoice.Id, Guid.NewGuid(), book.Title, 1, price)
        {
            Book = book
        };
}