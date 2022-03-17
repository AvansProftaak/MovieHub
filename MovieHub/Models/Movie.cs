using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Duration { get; set; }
    public string? Cast { get; set; } 
    public string? Director { get; set; } 
    [DisplayName("IMDb Score")]
    public double ImdbScore { get; set; } // In stead of Stars
    [DisplayName("Release date")]
    
    [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    [Column(TypeName="date")]
    public DateTime ReleaseDate { get; set; } 
    
    [DisplayName("3D")]
    public bool Is3D { get; set; }
    [DisplayName("Secret")]
    public bool IsSecret { get; set; }
    public string? Language { get; set; }
    [DisplayName("Image")]
    public string? ImageUrl { get; set; }
    [DisplayName("Trailer")]
    public string? TrailerUrl { get; set; }
    
    public IList<MoviePegi>? MoviePegis { get; set; }
    public IList<MovieGenre>? MovieGenres { get; set; }
    public IList<CinemaMovie>? CinemaMovies { get; set; }



    public Movie()
    {

    }
}