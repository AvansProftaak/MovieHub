namespace MovieHub.Models;

public class CinemaTickettype
{
    public int Id { get; set; }
    public int CinemaId { get; set; }
    public int TickettypeId { get; set; }
    
    public virtual Cinema? Cinema { get; set;  }
    public virtual Tickettype? Tickettype { get; set;  }

    public CinemaTickettype()
    {
        
    }
}