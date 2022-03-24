using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public bool Has3d { get; set; }
    public bool DolbySurround { get; set; }
    public bool WheelchairAccess { get; set; }

    [DisplayName( "Cinema")] 
    public int CinemaId { get; set; }
    [ForeignKey("CinemaId")]
    public virtual Cinema Cinema { get; set; } = null!;
    
}