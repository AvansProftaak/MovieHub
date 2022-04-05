using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Survey
{
    public int Id { get; set; }
    public int Age { get; set; }
    public GenderEnum Gender { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Facilities { get; set; }
    public int Hygiene { get; set; }
    public int FoodDrinks { get; set; }
    public int Staff { get; set; }
    public int ScreenQuality { get; set; }
    public int SoundQuality { get; set; }
    public int Price { get; set; }
    public string? Remark { get; set; }
    public DateTime SurveyFilledIn { get; set; }
    [ForeignKey("HallId")]
    public Hall Hall { get; set; } = null!;
    
    Survey()
    {
        
    }
}