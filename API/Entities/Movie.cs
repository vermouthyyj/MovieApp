
namespace API.Entities;

public class Movie
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Year { get; set; }
    public required string Type { get; set; }
    public required string Poster { get; set; }
}

public class MovieList
{
    public required List<Movie> Movies { get; set; }

    public static explicit operator MovieList(Task<IEnumerable<Movie>> v)
    {
        throw new NotImplementedException();
    }
}
