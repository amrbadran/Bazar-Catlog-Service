using Bazar_Catlog_Service.Data;
using Bazar_Catlog_Service.DTOs;
using Bazar_Catlog_Service.Services;
using Bazar_Catlog_Service.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Bazar_Catlog_Service.Api.Books;

public static class UpdateBooks
{
    public static void MapUpdateBooksEndpoints(this WebApplication app)
    {
        app.MapPatch("/books/cost/{bookNumber}", ChangeBookCost);
        app.MapPatch("/books/stock/{bookNumber}", IncreaseOrDecreaseBookStock);
    }

    public static async Task<IResult> ChangeBookCost(int bookNumber, BazarDbContext db, BookCostDTO bookCost, ReplicationService replicationService, CacheInvalidationService cacheInvalidationService, HttpContext httpContext)
    {
        try
        {
            var costValidator = new BookCostDTOValidator(bookCost);
            costValidator.Validate();

            var book = await db.Books.FindAsync(bookNumber);
            if (book == null)
            {
                return Results.NotFound("Book not found");
            }

            // Invalidate cache BEFORE write (server-push invalidation)
            var isReplication = httpContext.Request.Headers.ContainsKey("X-Replication");
            if (!isReplication)
            {
                await cacheInvalidationService.InvalidateBookCacheAsync(bookNumber);
            }

            book.Cost = bookCost.Cost!.Value;

            await db.SaveChangesAsync();

            if (!isReplication)
            {
                await replicationService.PropagateWriteAsync($"/books/cost/{bookNumber}", bookCost);
            }

            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    public static async Task<IResult> IncreaseOrDecreaseBookStock(int bookNumber, BazarDbContext db,
        BookStockDTO bookStock, ReplicationService replicationService, CacheInvalidationService cacheInvalidationService, HttpContext httpContext)
    {
        try
        {
            var stockValidator = new BookStockDTOValidator(bookStock);
            stockValidator.Validate();
            var  book = await db.Books.FindAsync(bookNumber);

            if (book == null)
            {
                return Results.NotFound("Book not found");
            }
            
            // Invalidate cache BEFORE write (server-push invalidation)
            var isReplication = httpContext.Request.Headers.ContainsKey("X-Replication");
            if (!isReplication)
            {
                await cacheInvalidationService.InvalidateBookCacheAsync(bookNumber);
            }
            
            var increase = bookStock.Increase;
            var decrease = bookStock.Decrease;

            if (increase.HasValue)
            {
                book.NumberOfItems += increase.Value;
            }

            if (decrease.HasValue)
            {
                book.NumberOfItems -= decrease.Value;
            }

            if (book.NumberOfItems < 0) throw new Exception("Number Of Items Will Be Negative");
            await db.SaveChangesAsync();

            if (!isReplication)
            {
                await replicationService.PropagateWriteAsync($"/books/stock/{bookNumber}", bookStock);
            }

            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new 
            {
                messsage = ex.Message
            });
        }
    }
}

