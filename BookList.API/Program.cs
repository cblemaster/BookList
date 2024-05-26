using BookList.API.Context;
using BookList.Core.DTO;
using BookList.Core.Entities;
using BookList.Core.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Context = BookList.API.Context.BookListContext;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .Build();

string connectionString = config.GetConnectionString("Project") ?? "Error retrieving connection string!";

builder.Services.AddDbContext<BookListContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Create Genre

// Delete Genre
app.MapDelete("/genre/{id:int}", async Task<Results<BadRequest<string>, NoContent, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid genre id.");
    }

    if (await context.Genres.Include(g => g.Books).SingleOrDefaultAsync(g => g.Id == id) is Genre genre)
    {
        if (genre.Books.Count != 0)
        {
            return TypedResults.BadRequest("Cannot delete genre since it is associated with one or more books.");
        }

        context.Genres.Remove(genre);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound("Unable to find genre to delete.");
});

// Update Genre
// Get Genres
app.MapGet("/genre", Results<NotFound<string>, Ok<IEnumerable<GenreDTO>>> (Context context) =>
{
    IEnumerable<GenreDTO> genres = EntityToDTO.MapGenreEntitiesToDTOs(context.Genres.AsEnumerable().OrderBy(g => g.Name));
    return genres is null || !genres.Any()
        ? TypedResults.NotFound("No genres found.")
        : TypedResults.Ok(genres.AsEnumerable());
});

// Get Genre By Id
app.MapGet("/genre/{id:int}", async Task<Results<BadRequest<string>, Ok<GenreDTO>, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid genre id.");
    }
    if (await context.Genres.SingleOrDefaultAsync(g => g.Id == id) is Genre genre)
    {
        GenreDTO dto = EntityToDTO.MapGenreEntityToDTO(genre);
        return TypedResults.Ok(dto);
    }
    return TypedResults.NotFound($"No genre with id {id} found.");
});

// Create Author

// Delete Author
app.MapDelete("/author/{id:int}", async Task<Results<BadRequest<string>, NoContent, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid author id.");
    }

    if (await context.Authors.Include(a => a.Books).SingleOrDefaultAsync(a => a.Id == id) is Author author)
    {
        if (author.Books.Count != 0)
        {
            return TypedResults.BadRequest("Cannot delete author since it is associated with one or more books.");
        }

        context.Authors.Remove(author);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound("Unable to find author to delete.");
});

// Update Author
// Get Authors
app.MapGet("/author", Results<NotFound<string>, Ok<IEnumerable<AuthorDTO>>> (Context context) =>
{
    IEnumerable<AuthorDTO> authors = EntityToDTO.MapAuthorEntitiesToDTOs(context.Authors.AsEnumerable().OrderBy(a => a.Name));
    return authors is null || !authors.Any()
        ? TypedResults.NotFound("No authors found.")
        : TypedResults.Ok(authors.AsEnumerable());
});

// Get Author By Id
app.MapGet("/author/{id:int}", async Task<Results<BadRequest<string>, Ok<AuthorDTO>, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid author id.");
    }
    if (await context.Authors.SingleOrDefaultAsync(a => a.Id == id) is Author author)
    {
        AuthorDTO dto = EntityToDTO.MapAuthorEntityToDTO(author);
        return TypedResults.Ok(dto);
    }
    return TypedResults.NotFound($"No author with id {id} found.");
});

// Create Book

// Delete Book
app.MapDelete("/book/{id:int}", async Task<Results<BadRequest<string>, NoContent, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid book id.");
    }

    if (await context.Books.Include(b => b.Authors).SingleOrDefaultAsync(b => b.Id == id) is Book book)
    {
        book.Authors.Clear();
        await context.SaveChangesAsync();
        context.Books.Remove(book);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound("Unable to find book to delete.");
});

// Update Book

// Get Books
app.MapGet("/book", Results<NotFound<string>, Ok<IEnumerable<BookDTO>>> (Context context) =>
{
    IEnumerable<BookDTO> books = EntityToDTO.MapBookEntitiesToDTOs(context.Books.Include(b => b.Genre).Include(b => b.Authors).OrderBy(b => b.Title));
    return books is null || !books.Any()
        ? TypedResults.NotFound("No books found.")
        : TypedResults.Ok(books.AsEnumerable());
});

// Get Book By Id
app.MapGet("/book/{id:int}", async Task<Results<BadRequest<string>, Ok<BookDTO>, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid book id.");
    }
    if (await context.Books.Include(b => b.Genre).Include(b => b.Authors).SingleOrDefaultAsync(b => b.Id == id) is Book book)
    {
        BookDTO dto = EntityToDTO.MapBookEntityToDTO(book);
        return TypedResults.Ok(dto);
    }
    return TypedResults.NotFound($"No book with id {id} found.");
});

app.Run();
