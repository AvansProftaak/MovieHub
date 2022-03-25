using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieHub.Models;

namespace MovieHub.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<MovieHub.Models.Movie> Movie { get; set; } = null!;
    public DbSet<MovieHub.Models.Pegi> Pegi { get; set; } = null!;
    public DbSet<MovieHub.Models.Genre> Genre { get; set; } = null!;
    public DbSet<MovieHub.Models.Cinema> Cinema { get; set; } = null!;
    public DbSet<MovieHub.Models.MoviePegi> MoviePegi { get; set; } = null!;
    public DbSet<MovieHub.Models.Hall> Hall { get; set; } = null!;
    public DbSet<MovieHub.Models.MovieGenre> MovieGenre { get; set; } = null!;
    public DbSet<MovieHub.Models.Tickettype> Tickettype { get; set; } = null!;
    public DbSet<MovieHub.Models.CinemaTickettype> CinemaTicketType { get; set; } = null!;
    public DbSet<MovieHub.Models.CinemaMovie> CinemaMovie { get; set; } = null!;
    public DbSet<MovieHub.Models.Seat> Seat { get; set; } = null!;
    public DbSet<MovieHub.Models.Showtime>? Showtime { get; set; } = null!;
    public DbSet<MovieHub.Models.CateringPackage> CateringPackage { get; set; } = null!;
    public DbSet<MovieHub.Models.Order> Order { get; set; } = null!;
    public DbSet<MovieHub.Models.Ticket> Ticket { get; set; } = null!;
    public DbSet<MovieHub.Models.User> User { get; set; } = null!;
    public DbSet<MovieHub.Models.Payment> Payment { get; set; } = null!;
    public DbSet<MovieHub.Models.PaymentMethod> PaymentMethod { get; set; } = null!;
    public DbSet<MovieHub.Models.MovieRuntime> MovieRuntime { get; set; } = null!;
    public DbSet<MovieHub.Models.Newsletter> Newsletter { get; set; } = null!;

}