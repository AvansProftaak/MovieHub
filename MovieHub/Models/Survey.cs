using System.ComponentModel;

namespace MovieHub.Models;

public class Survey
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    [DisplayName("Active")]
    public bool IsActive { get; set; }

    public Survey()
    {
        
    }
}