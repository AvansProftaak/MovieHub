using System.Collections.Generic;

namespace MovieHub.Models;

public class Pegi
{
    public int Id { get; set; }
    public string Description { get; set; } = null!; // e.g. 3, 7 or 18+ or Drugs, Discrimination, see: https://en.wikipedia.org/wiki/PEGI
    public string Icon { get; set; } = null!;
    
    public IList<MoviePegi>? MoviePegis { get; set; }
    
    public Pegi()
    {
        
    }
}