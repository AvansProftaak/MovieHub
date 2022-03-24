using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class MoviePegi
{
    public int Id { get; set; }

    [DisplayName( "Pegi")] 
    public int PegiId { get; set; }
    [ForeignKey("PegiId")]
    public virtual Pegi? Pegi { get; set;  }

    [DisplayName( "Movie")] 
    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public virtual Movie? Movie { get; set;  }

}