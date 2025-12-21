using Bazar_Catlog_Service.Data.Models;
using Bazar_Catlog_Service.Data;

namespace Bazar_Catlog_Service.Data.Seeding;

public class SeedBooks : IDisposable
{
    private readonly BazarDbContext _context;

    public SeedBooks(BazarDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        _context.Database.EnsureCreated();

        if (!_context.Books.Any())
        {
            _context.Books.AddRange(
                new Book()
                {
                    BookName = "How to get a good grade in DOS in 40 minutes a day", Cost = 30, NumberOfItems = 4,
                    Topic = "distributed systems"
                },
                new Book() { BookName = "RPCs for Noobs", Cost = 25, NumberOfItems = 6, Topic = "distributed systems" },
                new Book()
                {
                    BookName = "Xen and the Art of Surviving Undergraduate School.", Cost = 50, NumberOfItems = 10,
                    Topic = "undergraduate school"
                },
                new Book()
                {
                    BookName = "Cooking for the Impatient Undergrad", Cost = 30, NumberOfItems = 2,
                    Topic = "undergraduate school"
                },
                new Book()
                {
                    BookName = "How to finish Project 3 on time", Cost = 35, NumberOfItems = 5,
                    Topic = "undergraduate school"
                },
                new Book()
                {
                    BookName = "Why theory classes are so hard", Cost = 40, NumberOfItems = 3,
                    Topic = "undergraduate school"
                },
                new Book()
                {
                    BookName = "Spring in the Pioneer Valley", Cost = 45, NumberOfItems = 7,
                    Topic = "general"
                }
            );
            _context.SaveChanges();
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}