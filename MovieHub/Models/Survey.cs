using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Survey
{
    public int Id { get; set; }
    [Required]
    public int Age { get; set; }
    public GenderEnum Gender { get; set; }
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Name { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public int Facilities { get; set; }
    [Required]
    public int Hygiene { get; set; }
    [Required]
    public int FoodDrinks { get; set; }
    [Required]
    public int Staff { get; set; }
    [Required]
    public int ScreenQuality { get; set; }
    [Required]
    public int SoundQuality { get; set; }
    [Required]
    public int Price { get; set; }
    public string? Remark { get; set; }
    public DateTime SurveyFilledIn { get; set; }

    Survey()
    {
        
    }
}