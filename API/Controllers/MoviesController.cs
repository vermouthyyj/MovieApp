using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[ApiController]
[Route("[controller]")] // /movies
public class MoviesController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public MoviesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("x-access-token", "sjd1HfkjU83ksdsm3802k");
    }

    [HttpGet]
    public async Task<List<Movie>> GetMovies()
    {
        // Fetch movies from both providers asynchronously
        var moviesFromCinemaWorldTask = GetMoviesByProvider("cinemaworld");
        var moviesFromFilmWorldTask = GetMoviesByProvider("filmworld");

        // Wait for both tasks to complete
        await Task.WhenAll(moviesFromCinemaWorldTask, moviesFromFilmWorldTask);

        var moviesFromCinemaWorld = await moviesFromCinemaWorldTask;
        var moviesFromFilmWorld = await moviesFromFilmWorldTask;

        // Create a dictionary to store combined movies
        var combinedMoviesDict = new Dictionary<string, Movie>();

        // Combine movies from both providers
        CombineMovies(combinedMoviesDict, moviesFromCinemaWorld, "cw");
        CombineMovies(combinedMoviesDict, moviesFromFilmWorld, "fw");

        // Convert dictionary values to list
        var combinedMovies = combinedMoviesDict.Values.ToList();

        return combinedMovies;
    }

    private void CombineMovies(Dictionary<string, Movie> combinedMoviesDict, List<Movie> movies, string prefix)
    {
        foreach (var movie in movies)
        {
            // Extract ID without the first two letters
            var idWithoutPrefix = movie.Id.Substring(2);

            // Check if the movie ID exists in the combined movies dictionary
            if (combinedMoviesDict.ContainsKey(idWithoutPrefix))
            {
                // Update the movie if it's a better match
                if (movie.Id.Length < combinedMoviesDict[idWithoutPrefix].Id.Length)
                {
                    combinedMoviesDict[idWithoutPrefix] = movie;
                }
            }
            else
            {
                // Reconstruct the movie ID with the appropriate prefix
                combinedMoviesDict[idWithoutPrefix] = movie;
            }
        }
    }

    public async Task<List<Movie>?> GetMoviesByProvider(string provider)
    {
        string? apiUrl = GetProviderApiUrl(provider);


        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            // Deserialize the JSON response directly into a list of Movie objects
            var movies = JsonSerializer.Deserialize<MovieList>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return movies?.Movies ?? new List<Movie>(); // Return an empty list if movies is null
        }
        else
        {
            // Retry logic goes here
            await Task.Delay(1000); // Wait for 1 seconds before retrying
            return await GetMoviesByProvider(provider); // Recursive call
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