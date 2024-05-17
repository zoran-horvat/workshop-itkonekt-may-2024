using Bookstore.Data.Seeding;
using Bookstore.Data.Seeding.DataSeed;
using Bookstore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Bookstore.Domain.Invoices;
using Bookstore.Data;
using Bookstore.Data.Implementation;
using Bookstore.Domain.Models.BibliographicFormatters;
using Bookstore.Pages;
using Bookstore.Domain.Discounts;
using Bookstore.Domain.Discounts.Implementation;
using Bookstore.Domain.Common;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

ConfigurationManager configuration = builder.Configuration;

string bookstoreConnectionString =
    builder.Configuration.GetConnectionString("BookstoreConnection") ?? string.Empty;
builder.Services.AddDbContext<BookstoreDbContext>(options =>
    options.UseSqlServer(bookstoreConnectionString));

builder.Services.AddScoped<IUnitOfWork, BookstoreDbContext>();

builder.Services.AddScoped(typeof(ISpecification<>), typeof(QueryableSpecification<>));

builder.Services.AddScoped<InvoiceFactory>(_ => new InvoiceFactory(
    DateOnly.FromDateTime(DateTime.UtcNow),
    configuration.GetValue<int>("Invoicing:ToleranceDays", 30),
    configuration.GetValue<int>("Invoicing:DelinquencyDays", 10)));

var discount0 = new NoDiscount();

var discount1 = new RelativeDiscount(.1m);

var discount2 = new GiveForFree();

var usd10 = Currency.USD.Amount(10);
var discount3 = Cap.Relative(new RelativeDiscount(.5m), .1m);

var discount4 = Cap.Absolute(discount2, usd10);

var discount5 = ParallelDiscounts.Of(
    discount1,
    discount2,
    discount0);

var usd30 = Currency.USD.Amount(30);
var discount6 = 
    Cap.Absolute(
        new PriceFloor(usd30, new RelativeDiscount(.5m), new NoDiscount()),
        usd10);

builder.Services.AddSingleton<IDiscount>(_ => discount6);

builder.Services.AddSingleton<IAuthorNameFormatter, FullNameFormatter>();
builder.Services.AddSingleton<IAuthorListFormatter, SeparatedAuthorsFormatter>();
// builder.Services.AddSingleton<IBibliographicEntryFormatter, TitleOnlyFormatter>();
builder.Services.AddSingleton<IBibliographicEntryFormatter>(_ => TitleAndAuthorsFormatter.Academic());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IDataSeed<Person>, AuthorsSeed>();
    builder.Services.AddScoped<IDataSeed<Book>, BooksSeed>();
    builder.Services.AddScoped<IDataSeed<BookPrice>, BookPricesSeed>();
    builder.Services.AddScoped<IDataSeed<Customer>, CustomersSeed>();
    builder.Services.AddScoped<IDataSeed<InvoiceRecord>, InvoicesSeed>();
}
else
{
    builder.Services.AddScoped<IDataSeed<Person>, NoSeed<Person>>();
    builder.Services.AddScoped<IDataSeed<Book>, NoSeed<Book>>();
    builder.Services.AddScoped<IDataSeed<BookPrice>, NoSeed<BookPrice>>();
    builder.Services.AddScoped<IDataSeed<Customer>, NoSeed<Customer>>();
    builder.Services.AddScoped<IDataSeed<InvoiceRecord>, NoSeed<InvoiceRecord>>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
