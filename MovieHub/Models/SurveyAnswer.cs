using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class SurveyAnswer
{
    public int Id { get; set; }
    public string Answer { get; set; } = null!;
    [ForeignKey("SurveyQuestionId")] 
    public virtual SurveyQuestion SurveyQuestion { get; set; } = null!;
    
    public SurveyAnswer()
    {
        
    }
}