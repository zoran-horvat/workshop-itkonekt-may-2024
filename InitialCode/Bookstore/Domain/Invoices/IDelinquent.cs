using Bookstore.Domain.Common;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Invoices;

public interface IDelinquent
{
    IEnumerable<DueNotification> GetNotifications();
    IDelinquent Add(Invoice delinquentInvoice);

    public static IDelinquent Empty => new EmptyDelinquentList();
}

public record DueNotification(Customer Customer, Money Amount);
