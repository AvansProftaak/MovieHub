using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class OrderData
{
    public int movieId { get; set; }
    public int showtimeId { get; set; }

    public Dictionary<int,int> ticketTypes { get; set; }
    public Dictionary<int,int> cateringPackages { get; set; }
    public Dictionary<int,int> seats { get; set; }
    

    public OrderData()
    {
        
    }
}