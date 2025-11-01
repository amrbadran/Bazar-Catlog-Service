using Bazar_Catlog_Service.Api.Books;
using Bazar_Catlog_Service.Data;
using Bazar_Catlog_Service.Data.Seeding;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BazarDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Seed Data
using var seeding = new SeedBooks(app.Services.GetService<BazarDbContext>());
seeding.Seed();

app.MapQueryBooksEndpoints();
app.MapUpdateBooksEndpoints();
app.MapGet("/", () => "Hello World!");

app.Run();