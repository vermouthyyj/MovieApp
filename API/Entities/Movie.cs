namespace API.Entities;

public class Movie
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Year { get; set; }
    public required string Type { get; set; }
    public required string Poster { get; set; }
}
