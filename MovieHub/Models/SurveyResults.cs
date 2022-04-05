using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

[NotMapped]
public class SurveyResults
{
    public float Facilities { get; set; }
    public float Hygiene { get; set; }
    public float FoodDrinks { get; set; }
    public float Staff { get; set; }
    public float ScreenQuality { get; set; }
    public float SoundQuality { get; set; }
    public float Price { get; set; }
    
    public SurveyResults()
    {
    }
}