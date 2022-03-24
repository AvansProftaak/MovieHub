namespace MovieHub.Models;

public class OrderData
{
    public int MovieId { get; set; }
    public int ShowtimeId { get; set; }

    public Dictionary<string,int>? TicketTypes { get; set; }
    public Dictionary<string,int>? CateringPackages { get; set; }
    public List<List <string>>? Seats { get; set; }

}