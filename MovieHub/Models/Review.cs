using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Review
{
    public int Id { get; set; }
    public int CinemaId { get; set; }
    public int HallId { get; set; }
    
    public int DisplayQuality { get; set; }
    public int SoundQuality { get; set; }
    public int FoodQuality { get; set; }
    public int Disturbance { get; set; }
    public int Hygiene { get; set; }
    public string Name { get; set; } = null!;
    public string Email{ get; set; } = null!;
    
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
    public DateTime? TimeCreated { get; set; }
}
