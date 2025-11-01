using Bazar_Catlog_Service.Api.Books;
using Bazar_Catlog_Service.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<BazarDbContext>(options => options.UseSqlite("Data Source=Bazar.db"));

var app = builder.Build();

app.MapQueryBooksEndpoints();
app.MapGet("/", () => "Hello World!");

app.Run();