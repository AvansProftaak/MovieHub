using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieHub.Models;

namespace MovieHub.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Movie> Movie { get; set; } = null!;
    public DbSet<Pegi> Pegi { get; set; } = null!;
    public DbSet<Genre> Genre { get; set; } = null!;
    public DbSet<Cinema> Cinema { get; set; } = null!;
    public DbSet<MoviePegi> MoviePegi { get; set; } = null!;
    public DbSet<Hall> Hall { get; set; } = null!;
    public DbSet<MovieGenre> MovieGenre { get; set; } = null!;
    public DbSet<Tickettype> Tickettype { get; set; } = null!;
    public DbSet<CinemaTickettype> CinemaTicketType { get; set; } = null!;
    public DbSet<CinemaMovie> CinemaMovie { get; set; } = null!;
    public DbSet<Seat> Seat { get; set; } = null!;
    public DbSet<Showtime>? Showtime { get; set; } = null!;
    public DbSet<CateringPackage> CateringPackage { get; set; } = null!;
    public DbSet<Order> Order { get; set; } = null!;
    public DbSet<Ticket> Ticket { get; set; } = null!;
    public DbSet<Payment> Payment { get; set; } = null!;
    public DbSet<PaymentMethod> PaymentMethod { get; set; } = null!;
    public DbSet<MovieRuntime> MovieRuntime { get; set; } = null!;
    public DbSet<Newsletter> Newsletter { get; set; } = null!;

}