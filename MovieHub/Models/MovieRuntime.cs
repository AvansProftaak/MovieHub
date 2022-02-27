using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class MovieRuntime
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public virtual Movie Movie { get; set; }
    public int HallId { get; set; }
    [ForeignKey("HallId")]
    public virtual Hall Hall { get; set; }
    [DataType(DataType.Date)]
    [Column(TypeName="date")]
    public DateTime StartAt { get; set; }
    [DataType(DataType.Date)]
    [Column(TypeName="date")]
    public DateTime EndAt { get; set; }
    [DataType(DataType.Time)]
    [Column(TypeName="time")]
    public TimeSpan Time { get; set; }

    public MovieRuntime()
    {
        
    }
}