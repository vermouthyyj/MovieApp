using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[ApiController]
[Route("[controller]/{provider}")] // /movies
public class MovieController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public MovieController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("x-access-token", "sjd1HfkjU83ksdsm3802k");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovieDetail(string provider, string id)
    {
        string apiUrl = GetMovieApiUrl(provider, id);

        if (apiUrl == null)
        {
            return NotFound();
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode(); // Ensure success status code

            string content = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a Movie object
            var movie = JsonSerializer.Deserialize<MovieDetails>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(movie); // Return the movie details
        }
        catch (HttpRequestException ex)
        {
            // Log and return a 500 Internal Server Error response
            Console.WriteLine("HttpRequestException: " + ex.Message);
            return StatusCode(500, "Error accessing movie provider API.");
        }

    }

    private static string? GetMovieApiUrl(string provider, string id)
    {
        string baseUrl;
        switch (provider.ToLower())
        {
            case "filmworld":
                baseUrl = "http://webjetapitest.azurewebsites.net/api/filmworld/movie";
                break;
            case "cinemaworld":
                baseUrl = "http://webjetapitest.azurewebsites.net/api/cinemaworld/movie";
                break;
            default:
                return null;
        }

        // Construct the full API URL with the movie ID
        return $"{baseUrl}/{id}";
    }

}