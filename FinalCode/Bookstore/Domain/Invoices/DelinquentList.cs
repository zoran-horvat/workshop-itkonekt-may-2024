using System.Collections.Immutable;

namespace Bookstore.Domain.Invoices;

public class DelinquentList : IDelinquent
{
    private ImmutableDictionary<Guid, DelinquentCustomer> CustomerIdToInvoices { get; }

    private DelinquentList(ImmutableDictionary<Guid, DelinquentCustomer> customerIdToInvoices) => CustomerIdToInvoices = customerIdToInvoices;

    public DelinquentList(DelinquentCustomer customer)
        : this(ImmutableDictionary<Guid, DelinquentCustomer>.Empty.Add(customer.Customer.Id, customer)) { }

    public IDelinquent Add(Invoice delinquentInvoice) => new DelinquentList(this.AddToDictionary(delinquentInvoice));

    private ImmutableDictionary<Guid, DelinquentCustomer> AddToDictionary(Invoice invoice) =>
        this.CustomerIdToInvoices.TryGetValue(invoice.Customer.Id, out DelinquentCustomer? customer)
            ? this.CustomerIdToInvoices.SetItem(invoice.Customer.Id, customer.AddSameCustomer(invoice))
            : this.CustomerIdToInvoices.Add(invoice.Customer.Id, new DelinquentCustomer(invoice));

    public IEnumerable<DueNotification> GetNotifications() =>
        this.CustomerIdToInvoices.Values.SelectMany(customer => customer.GetNotifications());
}