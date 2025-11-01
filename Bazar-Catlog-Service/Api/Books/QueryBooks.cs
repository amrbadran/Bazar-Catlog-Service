using Bazar_Catlog_Service.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Bazar_Catlog_Service.Api.Books;

public static class QueryBooks
{
    public static void MapQueryBooksEndpoints(this WebApplication app)
    {
        app.MapGet("/search/{topicName}", async (string topicName, BazarDbContext db) =>
        {
            return await db.Books.Where(b => b.Topic == topicName).ToListAsync();
        });
    }
}