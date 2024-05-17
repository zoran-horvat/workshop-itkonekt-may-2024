using Bookstore.Data.Seeding;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.Data.Specifications;
using Bookstore.Domain.Discounts;

namespace Bookstore.Pages;

public class BookDetailsModel : PageModel
{
    public record PriceLine(string Label, Money Amount);

    private readonly ILogger<IndexModel> _logger;
    private readonly IUnitOfWork _dbContext;
    private readonly IDataSeed<BookPrice> _bookPricesSeed;
    private readonly ISpecification<Book> _allBooksSpec;
    private readonly IBibliographicEntryFormatter _recommendedBooksFormatter;

    public Book Book { get; private set; } = null!;

    private readonly IDiscount _discount;
    public IReadOnlyList<PriceLine> PriceSpecification { get; private set; } = Array.Empty<PriceLine>();

    public IReadOnlyList<(Citation book, Guid id)> RecommendedBooks { get; private set; } = Array.Empty<(Citation, Guid)>();

    public BookDetailsModel(ILogger<IndexModel> logger, IUnitOfWork dbContext, IDataSeed<BookPrice> bookPricesSeed,
                            IDiscount discount, ISpecification<Book> spec, IBibliographicEntryFormatter recommendedBooksFormatter) =>
        (_logger, _dbContext, _bookPricesSeed, _discount, _allBooksSpec, _recommendedBooksFormatter) =
        (logger, dbContext, bookPricesSeed, discount, spec, recommendedBooksFormatter);

    public async Task<IActionResult> OnGet(Guid id)
    {
        await _bookPricesSeed.SeedAsync();
        if ((await _dbContext.Books.SingleOrDefaultAsync(_allBooksSpec.ById(id))) is Book book)
        {
            this.Book = book;
            await this.PopulatePriceSpecification();
            await this.PopulateRecommendedBooks();
            return Page();
        }

        return Redirect("/books");
    }

    private async Task PopulatePriceSpecification()
    {
        Money? originalPrice = (await _dbContext.BookPrices.All.For(this.Book).At(DateTime.Now).FirstOrDefaultAsync())?.Price;
        this.PriceSpecification = originalPrice.HasValue ? this.CalculatePriceLines(originalPrice.Value) : new List<PriceLine>();
    }

    private async Task PopulateRecommendedBooks()
    {
        string[] words = this.Book.Title.SplitIntoWords().Where(word => word.Length > 3).ToArray();
        _logger.LogInformation("Title: {title}; words: {words}", this.Book.Title, string.Join(", ", words));
        var candidateBooks = await _dbContext.Books.All.ToListAsync();

        this.RecommendedBooks = candidateBooks
            .Select(book => (book, score: this.GetRecommendationScore(book, words)))
            .Where(bookScore => bookScore.book.Id != this.Book.Id && bookScore.score > 0)
            .OrderByDescending(bookScore => bookScore.score)
            .Take(3)
            .Select(bookScore => bookScore.book)
            .Select(book => (this._recommendedBooksFormatter.ToCitation(book), book.Id))
            .ToList();

        foreach (var recommended in this.RecommendedBooks)
        {
            _logger.LogInformation("Recommended: {title}", recommended.book);
        }
    }

    private int GetRecommendationScore(Book book, string[] targetWords) =>
        book.Title.SplitIntoWords().Intersect(targetWords).Count();

    private List<PriceLine> CalculatePriceLines(Money originalPrice)
    {
        List<PriceLine> priceLines = new();

        priceLines.Add(new("Original price", originalPrice));

        foreach (var application in _discount.ApplyTo(originalPrice))
        {
            priceLines.Add(new(application.Label, application.DiscountedAmount));
        }

        if (priceLines.Count > 1)
        {
            PriceLine finalPrice = priceLines.Aggregate((price, discount) => price with { Amount = price.Amount - discount.Amount }) with { Label = "Final price" };
            priceLines.Add(finalPrice);
        }

        return priceLines;
    }
}
