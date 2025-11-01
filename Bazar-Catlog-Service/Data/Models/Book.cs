namespace Bazar.Data.Models;

public class Book
{
    public int Id { get; set; }
    public string? BookName { get; set; }
    public int NumberOfItems { get; set; }
    public decimal Cost { get;set; }
    public string? Topic { get; set; }
}