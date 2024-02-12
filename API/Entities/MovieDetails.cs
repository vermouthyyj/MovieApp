namespace API.Entities;

public class MovieDetails
{
    public required string Id { get; set; }
    public required string Price { get; set; }
    public required string Rated { get; set; }
    public required string Released { get; set; }
    public required string Runtime { get; set; }
    public required string Genre { get; set; }
    public required string Director { get; set; }
    public required string Writer { get; set; }
    public required string Actors { get; set; }
    public required string Plot { get; set; }
    public required string Language { get; set; }
    public required string Country { get; set; }
    public required string Metascore { get; set; }
    public required string Rating { get; set; }
    public required string Votes { get; set; }
}