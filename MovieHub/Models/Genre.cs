namespace MovieHub.Models;

public class Genre
{
    public int Id { get; set; }
    public enum Category
    { 
        Action = 1,
        Animation = 2,
        Adventure = 3,
        Biography = 4,
        Comedy = 5,
        Crime = 6,
        Documentary = 7,
        Drama = 8,
        Family = 9,
        Fantasy = 10,
        Horror = 11,
        Music = 12,
        Mystery = 13,
        Romance = 14,
        Sciencefiction = 15,
        Sport = 16,
        Thriller = 17,
        War = 18,
        Western = 19
    }
    public IList<MovieGenre>? MovieGenres { get; set; }

    public Genre()
    {
    }
}