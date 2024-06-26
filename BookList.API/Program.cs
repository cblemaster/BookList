using BookList.API.Context;
using BookList.Core.DTO;
using BookList.Core.Entities;
using BookList.Core.Mappers;
using BookList.Core.Validation;
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
app.MapPost("/genre", async Task<Results<BadRequest<string>, Created<GenreDTO>>> (Context context, CreateUpdateGenreDTO genreToCreate) =>
{
    if (genreToCreate is null)
    {
        return TypedResults.BadRequest("No genre to create provided.");
    }

    if (context.Genres.Select(g => g.Name).Distinct().Contains(genreToCreate.Name))
    {
        return TypedResults.BadRequest($"Genre name {genreToCreate.Name} is already used.");
    }

    ValidationResult validationResult = genreToCreate.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    Genre newGenre = DTOToEntity.MapCreateUpdateGenreDTOToGenreEntity(genreToCreate);
    context.Genres.Add(newGenre);
    await context.SaveChangesAsync();

    return TypedResults.Created($"/genre/{newGenre.Id}", EntityToDTO.MapGenreEntityToDTO(newGenre));
});

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
app.MapPut("/genre/{id:int}", async Task<Results<BadRequest<string>, NoContent>> (Context context, int id, CreateUpdateGenreDTO dto) =>
{
    if (dto is null)
    {
        return TypedResults.BadRequest("No genre to update provided.");
    }

    if (id < 1 || id != dto.Id)
    {
        return TypedResults.BadRequest("Invalid genre id.");
    }

    ValidationResult validationResult = dto.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    Genre entity = (await context.Genres.SingleOrDefaultAsync(a => a.Id == id));

    if (entity is null)
    {
        return TypedResults.BadRequest("Unable to find genre to update.");
    }

    if (dto.Name != entity.Name && context.Genres.Select(a => a.Name).Distinct().Contains(dto.Name))
    {
        return TypedResults.BadRequest($"Genre name {dto.Name} is already used.");
    }

    entity = DTOToEntity.MapCreateUpdateGenreDTOToGenreEntity(dto, entity);
    
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

// Get Genres
app.MapGet("/genre", Results<NotFound<string>, Ok<IEnumerable<GenreDTO>>> (Context context) =>
{
    IEnumerable<GenreDTO> genres = EntityToDTO.MapGenreEntitiesToDTOs(context.Genres.AsEnumerable().OrderBy(g => g.Name));
    return genres is null || !genres.Any()
        ? TypedResults.NotFound("No genres found.")
        : TypedResults.Ok(genres.AsEnumerable());
});

// Get Genre with books by Id
app.MapGet("/genrewithbooks/{id:int}", async Task<Results<BadRequest<string>, Ok<GenreWithBooksDTO>, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid genre id.");
    }
    if (await context.Genres.Include(g => g.Books).SingleOrDefaultAsync(g => g.Id == id) is Genre genre)
    {
        GenreWithBooksDTO dto = EntityToDTO.MapGenreWithBooksEntityToDTO(genre);
        return TypedResults.Ok(dto);
    }
    return TypedResults.NotFound($"No genre with id {id} found.");
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
app.MapPost("/author", async Task<Results<BadRequest<string>, Created<AuthorDTO>>> (Context context, CreateUpdateAuthorDTO authorToCreate) =>
{
    if (authorToCreate is null)
    {
        return TypedResults.BadRequest("No author to create provided.");
    }

    if (context.Authors.Select(a => a.Name).Distinct().Contains(authorToCreate.Name))
    {
        return TypedResults.BadRequest($"Author name {authorToCreate.Name} is already used.");
    }

    ValidationResult validationResult = authorToCreate.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    Author newAuthor = DTOToEntity.MapCreateUpdateAuthorDTOToAuthorEntity(authorToCreate);
    context.Authors.Add(newAuthor);
    await context.SaveChangesAsync();

    return TypedResults.Created($"/author/{newAuthor.Id}", EntityToDTO.MapAuthorEntityToDTO(newAuthor));
});

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
app.MapPut("/author/{id:int}", async Task<Results<BadRequest<string>, NoContent>> (Context context, int id, CreateUpdateAuthorDTO dto) =>
{
    if (dto is null)
    {
        return TypedResults.BadRequest("No author to update provided.");
    }

    if (id < 1 || id != dto.Id)
    {
        return TypedResults.BadRequest("Invalid author id.");
    }
  
    ValidationResult validationResult = dto.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    Author entity = (await context.Authors.SingleOrDefaultAsync(a => a.Id == id));

    if (entity is null)
    {
        return TypedResults.BadRequest("Unable to find author to update.");
    }

    if (dto.Name != entity.Name && context.Authors.Select(a => a.Name).Distinct().Contains(dto.Name))
    {
        return TypedResults.BadRequest($"Author name {dto.Name} is already used.");
    }

    entity = DTOToEntity.MapCreateUpdateAuthorDTOToAuthorEntity(dto, entity);
    
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

// Get Authors
app.MapGet("/author", Results<NotFound<string>, Ok<IEnumerable<AuthorDTO>>> (Context context) =>
{
    IEnumerable<AuthorDTO> authors = EntityToDTO.MapAuthorEntitiesToDTOs(context.Authors.AsEnumerable().OrderBy(a => a.Name));
    return authors is null || !authors.Any()
        ? TypedResults.NotFound("No authors found.")
        : TypedResults.Ok(authors.AsEnumerable());
});

// Get Author with books by Id
app.MapGet("/authorwithbooks/{id:int}", async Task<Results<BadRequest<string>, Ok<AuthorWithBooksDTO>, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid author id.");
    }
    if (await context.Authors.Include(a => a.Books).SingleOrDefaultAsync(a => a.Id == id) is Author author)
    {
        AuthorWithBooksDTO dto = EntityToDTO.MapAuthorWithBooksEntityToDTO(author);
        return TypedResults.Ok(dto);
    }
    return TypedResults.NotFound($"No author with id {id} found.");
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
app.MapPost("/book", async Task<Results<BadRequest<string>, Created<BookDTO>>> (Context context, CreateUpdateBookDTO bookToCreate) =>
{
    if (bookToCreate is null)
    {
        return TypedResults.BadRequest("No book to create provided.");
    }

    ValidationResult validationResult = bookToCreate.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    Book newBook = DTOToEntity.MapCreateUpdateBookDTOToBookEntity(bookToCreate);
    newBook.Genre = await context.Genres.SingleOrDefaultAsync(g => g.Id == bookToCreate.Genre.Id);
    newBook.Authors.Clear();
    await context.SaveChangesAsync();
    foreach (AuthorDTO author in bookToCreate.Authors)
    {
        newBook.Authors.Add(await context.Authors.SingleOrDefaultAsync(a => a.Id == author.Id));
    }

    context.Books.Add(newBook);
    await context.SaveChangesAsync();

    return TypedResults.Created($"/book/{newBook.Id}", EntityToDTO.MapBookEntityToDTO(newBook));
});

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
app.MapPut("/book/{id:int}", async Task<Results<BadRequest<string>, NoContent>> (Context context, int id, CreateUpdateBookDTO dto) =>
{
    if (dto is null)
    {
        return TypedResults.BadRequest("No book to update provided.");
    }

    if (id < 1 || id != dto.Id)
    {
        return TypedResults.BadRequest("Invalid book id.");
    }

    ValidationResult validationResult = dto.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    Book entity = (await context.Books.Include(b => b.Authors).Include(b => b.Genre).SingleOrDefaultAsync(b => b.Id == id));

    if (entity is null)
    {
        return TypedResults.BadRequest("Unable to find book to update.");
    }

    entity = DTOToEntity.MapCreateUpdateBookDTOToBookEntity(dto, entity);
    entity.Genre = await context.Genres.SingleOrDefaultAsync(g => g.Id == dto.Genre.Id);
    entity.Authors.Clear();
    //await context.SaveChangesAsync();
    foreach (AuthorDTO author in dto.Authors)
    {
        entity.Authors.Add(await context.Authors.SingleOrDefaultAsync(a => a.Id == author.Id));
    }

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

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
