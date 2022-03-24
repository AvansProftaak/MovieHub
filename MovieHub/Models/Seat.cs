using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Seat
{
    public int? Id { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public bool Available { get; set; }
    
    [DisplayName( "Hall")] 
    public int HallId { get; set; } 
    [ForeignKey("HallId")]
    public virtual Hall Hall { get; set; } = null!;

}