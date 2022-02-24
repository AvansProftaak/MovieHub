namespace MovieHub.Models;

public class Genre
{
    public int Id { get; set; }
    public CategoryEnum Category { get; set; }
    
    public enum CategoryEnum
    { 
        Action,
        Animation,
        Adventure,
        Biography,
        Comedy,
        Crime,
        Documentary,
        Drama,
        Family,
        Fantasy,
        Horror, 
        Mystery,
        Romance,
        Sciencefiction,
        Sport,
        Thriller,
        War,
        Western
    }
    public IList<MovieGenre>? MovieGenres { get; set; }

    public Genre()
    {
    }
}