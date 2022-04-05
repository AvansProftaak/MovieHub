using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieHub.Models;

public class Survey
{
    public int Id { get; set; }
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Title { get; set; }
    [StringLength(255, MinimumLength = 3)]
    [Required]
    public string? Description { get; set; }
    [DisplayName("Active")]
    public bool IsActive { get; set; }

    public Survey()
    {
        
    }
}