namespace Bookstore.Domain.Invoices;

public class EmptyDelinquentList : IDelinquent
{
    public IDelinquent Add(Invoice delinquentInvoice) => new DelinquentCustomer(delinquentInvoice);

    public IEnumerable<DueNotification> GetNotifications() => Enumerable.Empty<DueNotification>();
}