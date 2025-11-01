// Test Seeding Books

using Bazar.Data;
using Bazar.Data.Seeding;

var context = new BazarDbContext();
using var seedBooks = new SeedBooks(context);
seedBooks.Seed();

foreach (var book in context.Books)
{
    Console.WriteLine(book.BookName);
}
