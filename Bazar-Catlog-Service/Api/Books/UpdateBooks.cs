using Bazar_Catlog_Service.Data;
using Bazar_Catlog_Service.DTOs;
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

    public static async Task<IResult> ChangeBookCost(int bookNumber, BazarDbContext db, BookCostDTO bookCost)
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

            book.Cost = bookCost.Cost!.Value;

            await db.SaveChangesAsync();

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
        BookStockDTO bookStock)
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
            await db.SaveChangesAsync();
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