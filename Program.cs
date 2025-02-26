using Microsoft.EntityFrameworkCore;
using TestProjectLibrary;
using TestProjectLibrary.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Library"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedDatabase(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedDatabase(AppDbContext dbContext)
{
    if (!dbContext.Authors.Any())
    {
        var authors = new List<Author>
        {
            new Author { Id = 1, Name = "Stephen King", BirthDate = new DateTime(1947, 9, 21) },
            new Author { Id = 2, Name = "J.K. Rowling", BirthDate = new DateTime(1965, 7, 31) }
        };

        var books = new List<Book>
        {
            new Book { Id = 1, Title = "It", AuthorId = 1, Year = 1986 },
            new Book { Id = 2, Title = "The Shining", AuthorId = 1, Year = 1977 },
            new Book { Id = 3, Title = "Harry Potter and the Philosopher's Stone", AuthorId = 2, Year = 1997 }
        };

        dbContext.Authors.AddRange(authors);
        dbContext.Books.AddRange(books);
        dbContext.SaveChanges();
    }
}