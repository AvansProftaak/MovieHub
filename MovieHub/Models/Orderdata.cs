using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class OrderData
{
    public int movieId { get; set; }
    public int showtimeId { get; set; }

    public Dictionary<string,int> ticketTypes { get; set; }
    public Dictionary<string,int> cateringPackages { get; set; }
    public Dictionary<string,string> seats { get; set; }
    

    public OrderData()
    {
        
    }
}