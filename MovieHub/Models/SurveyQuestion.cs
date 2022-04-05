namespace MovieHub.Models;

public class SurveyQuestion
{
    public int Id { get; set; }
    public string Question { get; set; } = null!;
    public string Description { get; set; } = null!;

    public SurveyQuestion()
    {
        
    }
}