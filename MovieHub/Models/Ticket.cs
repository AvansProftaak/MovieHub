namespace MovieHub.Models;

public class Ticket
{
    public int Id { get; set; }
    public int Barcode { get; set; }
    public int OrderId { get; set; }
    public double Price { get; set; }
    public int TickettypeId { get; set; }
    public int SeatId { get; set; }
    public enum Status 
    {
        Open = 1,
        Pending = 2,
        Paid = 3
    }
}