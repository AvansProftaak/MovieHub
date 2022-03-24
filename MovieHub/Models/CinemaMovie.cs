using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class CinemaMovie
{
    public int Id { get; set; }
 
    [DisplayName( "Movie")] 
    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public virtual Movie? Movie { get; set;  } 

    [DisplayName( "Cinema")] 
    public int CinemaId { get; set; }
    [ForeignKey("CinemaId")]
    public virtual Cinema? Cinema { get; set;  }

}