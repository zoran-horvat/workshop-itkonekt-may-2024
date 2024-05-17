using System.Collections.Immutable;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Invoices;

public class DelinquentCustomer : IDelinquent
{
    private ImmutableList<Invoice> Invoices { get; }
    public Customer Customer => Invoices[0].Customer;

    private DelinquentCustomer(ImmutableList<Invoice> invoices) => Invoices = invoices;

    public DelinquentCustomer(Invoice invoice) : this(ImmutableList.Create(invoice)) { }

    public IEnumerable<DueNotification> GetNotifications()
    {
        yield return new DueNotification(this.Customer, this.Invoices.Aggregate(Money.Zero, (total, invoice) => total + invoice.Total));
    }

    public IDelinquent Add(Invoice delinquentInvoice) =>
        delinquentInvoice.Customer.Id == this.Customer.Id ? new DelinquentCustomer(this.Invoices.Add(delinquentInvoice))
        : new DelinquentList(this).Add(delinquentInvoice);

    public DelinquentCustomer AddSameCustomer(Invoice delinquentInvoice) =>
        delinquentInvoice.Customer.Id == this.Customer.Id ? new DelinquentCustomer(this.Invoices.Add(delinquentInvoice))
        : throw new ArgumentException("Cannot add invoice from different customer", nameof(delinquentInvoice));
}
