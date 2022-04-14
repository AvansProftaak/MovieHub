using MovieHub.Models;

namespace MovieHub.ViewModels
{
    public class CashierViewModel
    {
        public List<Ticket> tickets { get; set; }
        public List<Seat> seats { get; set; }
        public List<Order> orders { get; set; }
    }
}
