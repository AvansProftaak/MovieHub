namespace MovieHub.Models;

public class CateringPackage
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string Description { get; set; } = null!;
    public int Amount { get; set; }

    public CateringPackage()
    {
        
    }
}