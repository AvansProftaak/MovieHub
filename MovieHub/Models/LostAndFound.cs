using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class LostAndFound
{
    public int Id { get; set; }
    
    [DisplayName("Issue date")]
    [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = false)]
    [DataType(DataType.Date)]
    [Column(TypeName="date")]
    public DateTime IssueDate { get; set; } 
    
    public string Find { get; set; }
    
    public string Description { get; set; }
    
    public Boolean Collected { get; set; }
    
    
    public LostAndFound()
    {
        
    }
}