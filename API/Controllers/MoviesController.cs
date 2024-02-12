using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[ApiController]
[Route("[controller]/{provider}")] // /movies
public class MoviesController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public MoviesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("x-access-token", "sjd1HfkjU83ksdsm3802k");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(string provider)
    {
        string apiUrl = GetProviderApiUrl(provider);

        if (apiUrl == null)
        {
            return NotFound();
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode(); // Ensure success status code

            string content = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response directly into a list of Movie objects
            var movies = JsonSerializer.Deserialize<MovieData>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (movies == null)
            {
                return Ok(null);
            }

            return Ok(movies.Movies); // Return the list of movies
        }
        catch (HttpRequestException ex)
        {
            // Log and return a 500 Internal Server Error response
            Console.WriteLine("HttpRequestException: " + ex.Message);
            return StatusCode(500, "Error accessing movie provider API.");
        }

    }

    private static string? GetProviderApiUrl(string provider)
    {
        switch (provider.ToLower())
        {
            case "filmworld":
                return "http://webjetapitest.azurewebsites.net/api/filmworld/movies";
            case "cinemaworld":
                return "http://webjetapitest.azurewebsites.net/api/cinemaworld/movies";
            default:
                return null;
        }
    }

}