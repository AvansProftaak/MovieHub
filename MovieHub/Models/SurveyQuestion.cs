using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class SurveyQuestion
{
    public int Id { get; set; }
    public string Question { get; set; } = null!;
    public QuestionTypeEnum QuestionType { get; set; }
    [ForeignKey("SurveyId")] 
    public virtual Survey Survey { get; set; }
    
    public SurveyQuestion()
    {
        
    }
}