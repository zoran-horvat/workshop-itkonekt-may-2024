using Bookstore.Data.Seeding;
using Bookstore.Domain.Common;
using Bookstore.Domain.Invoices;
using Bookstore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class InvoicesModel : PageModel
{
    public record InvoiceRow(Guid Id, int Ordinal, string Label, string IssueDate, string Status, Money Total, string Style, bool AllowPayment);
    public record DelinquentRow(int Ordinal, string IssuedTo, Money Amount);

    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _context;
    private readonly IDataSeed<InvoiceRecord> _invoicesSeed;
    private readonly InvoiceFactory _invoiceFactory;

    public IEnumerable<InvoiceRow> Invoices { get; private set; } = Enumerable.Empty<InvoiceRow>();
    public IEnumerable<DelinquentRow> DelinquentCustomers { get; private set; } = Enumerable.Empty<DelinquentRow>();

    public InvoicesModel(ILogger<IndexModel> logger, BookstoreDbContext context, IDataSeed<InvoiceRecord> invoicesSeed, InvoiceFactory invoiceFactory) => 
        (_logger, _context, _invoicesSeed, _invoiceFactory) = (logger, context, invoicesSeed, invoiceFactory);

    public async Task OnGet()
    {
        await _invoicesSeed.SeedAsync();
        await PopulateInvoices();
        await PopulateDelinquentCustomers();
    }

    public async Task<IActionResult> OnPost(Guid invoiceId)
    {
        if (_context.Invoices.Find(invoiceId) is InvoiceRecord record)
        {
            record.PaymentTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Invoice {invoiceId} paid", invoiceId);
        }
        else
        {
            _logger.LogWarning("Invoice {invoiceId} not found", invoiceId);
        }
        return RedirectToPage("/invoices");
    }

    private async Task PopulateInvoices()
    {
        var records = await _context.Invoices
            .Include(invoice => invoice.Customer)
            .Include(invoice => invoice.Lines)
            .OrderBy(invoice => invoice.IssueTime)
            .ToListAsync();
        var invoices = _invoiceFactory.ToModels(records);

        this.Invoices = invoices.OrderBy(invoice => invoice.IssueDate).Select((record, index) => ToRow(index + 1, record)).ToList();
    }

    private async Task PopulateDelinquentCustomers() => 
        this.DelinquentCustomers = (await GetDelinquentCustomers())
            .GetNotifications()
            .OrderBy(notification => notification.Customer.Label)
            .Select((notification, index) => new DelinquentRow(index + 1, notification.Customer.Label, notification.Amount))
            .ToList();

    private static InvoiceRow ToRow(int ordinal, Invoice invoice) => new(
        invoice.Id, ordinal, invoice.Label,
        invoice.IssueDate.ToString("MM/dd/yyyy"),
        $"{invoice.Status.prefix} {invoice.Status.date}",
        invoice.Total, ToStyle(invoice), invoice is UnpaidInvoice);

    private static string ToStyle(Invoice invoice) =>
        invoice.GetType().Name.Replace("Invoice", "").ToLower();

    private async Task<IDelinquent> GetDelinquentCustomers() =>
        _invoiceFactory
            .ToModels(await GetDelinquentInvoiceRecords())
            .Aggregate(IDelinquent.Empty, (delinquent, invoice) => delinquent.Add(invoice));

    private async Task<IEnumerable<InvoiceRecord>> GetDelinquentInvoiceRecords() =>
        await _context.Invoices
            .Include(invoice => invoice.Customer)
            .Include(invoice => invoice.Lines)
            .Where(_invoiceFactory.DelinquentInvoiceTest)
            .ToListAsync();
}
