using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class CinemaTickettype
{
    public int Id { get; set; }
    
    [DisplayName( "Cinema")] 
    public int CinemaId { get; set; }
    [ForeignKey("CinemaId")]
    public virtual Cinema? Cinema { get; set;  } 

    [DisplayName( "Tickettype")] 
    public int TickettypeId { get; set; }
    [ForeignKey("TickettypeId")]
    public virtual Tickettype? Tickettype { get; set;  }

    public CinemaTickettype()
    {
        
    }
}