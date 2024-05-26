﻿using BookList.Core.DTO;
using System.Net.Http.Json;

namespace BookList.Core.Services;

public class HttpDataService : IDataService
{
    private readonly HttpClient _client;
    private const string BASE_URI = "https://localhost:7138/";

    public HttpDataService() => _client = new HttpClient
    {
        BaseAddress = new Uri(BASE_URI)
    };

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

    public async Task DeleteGenreAsync(int id)
    {
        if (id < 1) { return; }

        GenreDTO genre = await GetGenreAsync(id);

        if (genre is not null)
        {
            if (await DoesGenreHaveBooks(id)) { return; }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/genre/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }
    }

    public async Task DeleteAuthorAsync(int id)
    {
        if (id < 1) { return; }

        AuthorDTO author = await GetAuthorAsync(id);

        if (author is not null)
        {
            if (await DoesAuthorHaveBooks(id)) { return; }

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/author/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }
    }

    public async Task<bool> DoesGenreHaveBooks(int id) =>
        (await GetBooksAsync())
                .Select(b => b.Genre)
                .Select(g => g.Id)
                .Contains(id);

    public async Task<bool> DoesAuthorHaveBooks(int id)
    {
        List<int> authorIds = [];
        foreach (BookDTO book in await GetBooksAsync())  //todo: refactor using linq
        {
            foreach (AuthorDTO auth in book.Authors)
            {
                authorIds.Add(auth.Id);
            }
        }
        return authorIds.Contains(id);
    }

}
