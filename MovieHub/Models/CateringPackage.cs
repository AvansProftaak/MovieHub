using MovieHub.Data;

namespace MovieHub.Models;

public class CateringPackage
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    
    public int Quantity { get; set; } 

    public CateringPackage()
    {
        
    }
}