using Bookstore.Domain.Models;

namespace Bookstore.Data;

public interface IUnitOfWork : IDisposable
{
    public IRepository<Book> Books { get; }
    public IRepository<BookPrice> BookPrices { get; }
    void Commit();
}
