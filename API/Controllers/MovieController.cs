using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[ApiController]
[Route("[controller]")] // /movies
public class MovieController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private object movieDetail;

    public MovieController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("x-access-token", "sjd1HfkjU83ksdsm3802k");
    }

    [HttpGet("{id}/price")]
    // Method to retrieve the movie price by its ID
    public async Task<ActionResult<string>> GetMoviePrice(string id)
    {
        // Get provide by movie id
        string provider = getProviderById(id);
        if (provider == "invalid id")
        {
            return BadRequest("Invalid movie ID");
        }

        // Call the GetMovieDetail method to retrieve the movie details including the price
        var movieDetailsResponse = await GetMovieDetail(provider, id);

        if (movieDetailsResponse.Result is OkObjectResult okObjectResult)
        {
            // Extract the movie details with the price from the response
            var movieDetailsWithPrice = okObjectResult.Value as MovieDetails;

            // Return the price extracted from the movie details
            return Ok(movieDetailsWithPrice?.Price);
        }
        else
        {
            // Return the response received from the GetMovieDetail method
            return movieDetailsResponse.Result;
        }
    }

    private string getProviderById(string id)
    {
        string prefix = id.Substring(0, 2).ToLower();
        Console.WriteLine("Prefix: " + prefix);
        switch (prefix)
        {
            case "cw":
                return "cinemaworld";
            case "fw":
                return "filmworld";
            default:
                return "invalid id";
        }
    }

    [HttpGet("{provider}/{id}")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovieDetail(string provider, string id)
    {
        string? apiUrl = GetMovieApiUrl(provider, id);

        if (apiUrl == null)
        {
            return NotFound();
        }

        HttpResponseMessage? response = null;
        int maxRetries = 3;
        int retryCount = 0;
        bool success = false;

        while (!success && retryCount < maxRetries)
        {
            try
            {
                response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Ensure success status code

                success = true;
            }
            catch (HttpRequestException ex)
            {
                // Log and return a 500 Internal Server Error response
                Console.WriteLine("HttpRequestException: " + ex.Message);
                retryCount++;
                if (retryCount < maxRetries)
                {
                    Console.WriteLine($"Retrying... Attempt {retryCount} of {maxRetries}");
                    await Task.Delay(1000); // Wait for 1 second before retrying
                }
            }
        }

        // Success
        if (response != null && response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();

            var movie = JsonSerializer.Deserialize<MovieDetails>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(movie); // Return the movie details
        }
        // Error handling
        else
        {
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