using Bazar_Catlog_Service.Data;
using Bazar_Catlog_Service.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Bazar_Catlog_Service.Api.Books;

public static class QueryBooks
{
    public static void MapQueryBooksEndpoints(this WebApplication app)
    {
        app.MapGet("books/search/{topicName}", async (string topicName, BazarDbContext db) =>
        {
            return await db.Books.Where(b => b.Topic == topicName).ToListAsync();
        });

        app.MapGet("books/info/{bookNumber}", async (int bookNumber, BazarDbContext db) =>
        {
            return await db.Books.Where(b => b.Id == bookNumber).FirstOrDefaultAsync();
        });
    }
}