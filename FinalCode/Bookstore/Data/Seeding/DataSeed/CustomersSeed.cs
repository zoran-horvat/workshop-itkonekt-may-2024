using Bookstore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Seeding.DataSeed;

public class CustomersSeed : IDataSeed<Customer>
{
    private readonly BookstoreDbContext _dbContext;

    public CustomersSeed(BookstoreDbContext dbContext) => _dbContext = dbContext;

    public async Task<Customer> EnsureEqualExists(Customer entity)
    {
        if (_dbContext.Customers.FirstOrDefault(c => c.Label == entity.Label) is Customer existing) return existing;

        _dbContext.Customers.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task SeedAsync()
    {
        if (await _dbContext.Customers.AnyAsync()) return;

        foreach (Customer customer in DemoCustomers)
        {
            await EnsureEqualExists(customer);
        }
    }

    private static IEnumerable<Customer> DemoCustomers => new Customer[]
    {
        Customer.CreateNew("Joe"),
        Customer.CreateNew("Jill"),
        Customer.CreateNew("Jake"),
        Customer.CreateNew("Jim"),
        Customer.CreateNew("Jane"),
        Customer.CreateNew("Jerry"),
        Customer.CreateNew("Jenny"),
        Customer.CreateNew("Jesse"),
    };
}