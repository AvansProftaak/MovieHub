using System.ComponentModel.DataAnnotations;

namespace MovieHub.Models;

public class Survey
{
    public int Id { get; set; }
    public int CinemaNumber { get; set; }
    public int TicketPrice { get; set; }
    public int ScreenQuality { get; set; }
    public int SoundQuality { get; set; }
    public int PopcornQuality { get; set; }
    public int Nuisance { get; set; }
    public int Hygiene { get; set; }
    public int ToiletHeight { get; set; } 
    public string Name { get; set; } = null!;
    public string Email{ get; set; } = null!;
    public DateTime? TimeStamp { get; set; }
}