using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Review
{
    public int Id { get; set; }
    public int ScreenQuality { get; set; }
    public int SoundQuality { get; set; }
    public int PopcornQuality { get; set; }
    public int Nuisance { get; set; }
    public int Hygiene { get; set; }
    public string Name { get; set; } = null!;
    public string Email{ get; set; } = null!;
    
    [DisplayName( "Cinema")] 
    public int CinemaId { get; set; }
    [ForeignKey("CinemaId")]
    public virtual Cinema Cinema { get; set; } = null!;
    
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
    public DateTime? TimeCreated { get; set; }
}
