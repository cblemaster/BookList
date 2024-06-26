﻿using BookList.Core.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace BookList.Core.Services;

public class HttpDataService : IDataService
{
    private readonly HttpClient _client;
    private const string BASE_URI = "https://localhost:7138/";

    public HttpDataService() => _client = new HttpClient
    {
        BaseAddress = new Uri(BASE_URI)
    };

    public async Task<AuthorDTO> CreateAuthorAsync(CreateUpdateAuthorDTO authorToCreate)
    {
        if (await IsAuthorNameAlreadyUsed(authorToCreate.Name))
        { return new(0, "Author name already used.", false); }

        StringContent content = new(JsonSerializer.Serialize(authorToCreate));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync("/author", content);
            response.EnsureSuccessStatusCode();
            return await GetAuthorAsync(await response.Content.ReadFromJsonAsync<AuthorDTO>() is AuthorDTO newAuthor ? newAuthor.Id : 0);
        }
        catch (Exception) { throw; }
    }

    public async Task DeleteAuthorAsync(int id)
    {
        if (id < 1) { return; }

        AuthorDTO author = await GetAuthorAsync(id);

        if (author is not null)
        {
            if ((await GetAuthorWithBooksAsync(id)).Books.Any()) { return; }

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/author/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }
    }

    public async Task<IEnumerable<AuthorDTO>> GetAuthorsAsync()
    {
        IEnumerable<AuthorDTO> authors = [];

        try
        {
            HttpResponseMessage response = await _client.GetAsync("/author");
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                authors = response.Content.ReadFromJsonAsAsyncEnumerable<AuthorDTO>().ToBlockingEnumerable();
            }
            return authors is not null ? authors : [];
        }
        catch (Exception) { throw; }
    }

    public async Task<AuthorDTO> GetAuthorAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"/author/{id}");
            return response.IsSuccessStatusCode && response.Content is not null
                ? await response.Content.ReadFromJsonAsync<AuthorDTO>() ?? new(0, "NotFound", false)
                : new(0, "NotFound", false);
        }
        catch (Exception) { throw; }
    }

    public async Task<AuthorWithBooksDTO> GetAuthorWithBooksAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"/authorwithbooks/{id}");
            return response.IsSuccessStatusCode && response.Content is not null
                ? await response.Content.ReadFromJsonAsync<AuthorWithBooksDTO>() ?? new(0, "NotFound", false, Enumerable.Empty<BookOnlyDTO>())
                : new(0, "NotFound", false, Enumerable.Empty<BookOnlyDTO>());
        }
        catch (Exception) { throw; }
    }

    public async Task UpdateAuthorAsync(int id, CreateUpdateAuthorDTO dto)
    {
        if (dto is null || id < 1 || id != dto.Id) { return; }

        if ((await GetAuthorAsync(id)).Name != dto.Name
            && await IsAuthorNameAlreadyUsed(dto.Name)) 
            { return; }

        StringContent content = new(JsonSerializer.Serialize(dto));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PutAsync($"/author/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception) { throw; }
    }

    public async Task<BookDTO> CreateBookAsync(CreateUpdateBookDTO bookToCreate)
    {
        StringContent content = new(JsonSerializer.Serialize(bookToCreate));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync("/book", content);
            response.EnsureSuccessStatusCode();
            return await GetBookAsync(await response.Content.ReadFromJsonAsync<BookDTO>() is BookDTO newBook ? newBook.Id : 0);
        }
        catch (Exception) { throw; }
    }

    public async Task DeleteBookAsync(int id)
    {
        if (id < 1) { return; }

        BookDTO book = await GetBookAsync(id);

        if (book is not null)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/book/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; };
        }
    }

    public async Task<IEnumerable<BookDTO>> GetBooksAsync()
    {
        IEnumerable<BookDTO> books = [];

        try
        {
            HttpResponseMessage response = await _client.GetAsync("/book");
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                books = response.Content.ReadFromJsonAsAsyncEnumerable<BookDTO>().ToBlockingEnumerable();
            }
            return books is not null ? books : [];
        }
        catch (Exception) { throw; }
    }

    public async Task<BookDTO> GetBookAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"/book/{id}");
            return response.IsSuccessStatusCode && response.Content is not null
                ? await response.Content.ReadFromJsonAsync<BookDTO>()
                    ?? new(0, "NotFound", string.Empty, false, null, null,
                       null, new(0, "NotFound", false), [new(0, "NotFound", false)])
                : new(0, "NotFound", string.Empty, false, null, null, null,
                    new(0, "NotFound", false), [new(0, "NotFound", false)]);
        }
        catch (Exception) { throw; }
    }

    public async Task UpdateBookAsync(int id, CreateUpdateBookDTO dto)
    {
        if (dto is null || id < 1 || id != dto.Id) { return; }

        StringContent content = new(JsonSerializer.Serialize(dto));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PutAsync($"/book/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception) { throw; }
    }

    public async Task<GenreDTO> CreateGenreAsync(CreateUpdateGenreDTO genreToCreate)
    {
        if (await IsGenreNameAlreadyUsed(genreToCreate.Name))
        { return new(0, "Genre name already used.", false); }

        StringContent content = new(JsonSerializer.Serialize(genreToCreate));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync("/genre", content);
            response.EnsureSuccessStatusCode();
            return await GetGenreAsync(await response.Content.ReadFromJsonAsync<GenreDTO>() is GenreDTO newGenre ? newGenre.Id : 0);
        }
        catch (Exception) { throw; }
    }

    public async Task DeleteGenreAsync(int id)
    {
        if (id < 1) { return; }

        GenreDTO genre = await GetGenreAsync(id);

        if (genre is not null)
        {
            if ((await GetGenreWithBooksAsync(id)).Books.Any()) { return; }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/genre/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }
    }

    public async Task<IEnumerable<GenreDTO>> GetGenresAsync()
    {
        IEnumerable<GenreDTO> genres = [];

        try
        {
            HttpResponseMessage response = await _client.GetAsync("/genre");
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                genres = response.Content.ReadFromJsonAsAsyncEnumerable<GenreDTO>().ToBlockingEnumerable();
            }
            return genres is not null ? genres : [];
        }
        catch (Exception) { throw; }
    }

    public async Task<GenreDTO> GetGenreAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"/genre/{id}");
            return response.IsSuccessStatusCode && response.Content is not null
                ? await response.Content.ReadFromJsonAsync<GenreDTO>() ?? new(0, "NotFound", false)
                : new(0, "NotFound", false);
        }
        catch (Exception) { throw; }
    }

    public async Task<GenreWithBooksDTO> GetGenreWithBooksAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"/genrewithbooks/{id}");
            return response.IsSuccessStatusCode && response.Content is not null
                ? await response.Content.ReadFromJsonAsync<GenreWithBooksDTO>() ?? new(0, "NotFound", false, Enumerable.Empty<BookOnlyDTO>())
                : new(0, "NotFound", false, Enumerable.Empty<BookOnlyDTO>());
        }
        catch (Exception) { throw; }
    }

    public async Task UpdateGenreAsync(int id, CreateUpdateGenreDTO dto)
    {
        if (dto is null || id < 1 || id != dto.Id) { return; }

        if ((await GetGenreAsync(id)).Name != dto.Name
            && await IsAuthorNameAlreadyUsed(dto.Name))
            { return; }

        StringContent content = new(JsonSerializer.Serialize(dto));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PutAsync($"/genre/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception) { throw; }
    }

    public async Task<bool> IsAuthorNameAlreadyUsed(string name)
    {
        IEnumerable<string> existingAuthorNames = (await GetAuthorsAsync())
            .Select(a => a.Name).Distinct();
        return existingAuthorNames is not null && existingAuthorNames.Contains(name);
    }

    public async Task<bool> IsGenreNameAlreadyUsed(string name)
    {
        IEnumerable<string> existingGenreNames = (await GetGenresAsync())
            .Select(g => g.Name).Distinct();
        return existingGenreNames is not null && existingGenreNames.Contains(name);
    }
}
