using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MovieHubTests
{
    public class CashierDashboardControllerTests
    {
        private readonly CashierDashboardController _controller;
        private readonly ApplicationDbContext _context;

        public CashierDashboardControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("HomeTestDatabase").Options;
            _context = new ApplicationDbContext(options);
            var logger = Mock.Of<ILogger<CashierDashboardController>>();
            _controller = new CashierDashboardController(logger, _context);
            InsertTestData(_context);
        }

        [Fact]
        public async Task Delete_Ticket_Test()
        {
            var testTicket = await _context.Ticket.FirstOrDefaultAsync();
            Assert.NotNull(testTicket);
            await _controller.DeleteTicket(1);
            testTicket = await _context.Ticket.FirstOrDefaultAsync();
            Assert.Null(testTicket);
        }

        [Fact]
        public async Task Delete_Order_Test()
        {
            var testOrder = await _context.Order.FirstOrDefaultAsync();
            Assert.NotNull(testOrder);
            await _controller.DeleteOrder(1);
            testOrder = await _context.Order.FirstOrDefaultAsync();
            Assert.Null(testOrder);
        }

        [Fact]
        public async Task Change_Seat_Test()
        {

            var testTicket = await _context.Ticket.FirstOrDefaultAsync();
            Assert.Equal(1, testTicket.Seat.Id);
            await _controller.ChangeSeat(2, 2, 1, 1);
            Assert.Equal(2, testTicket.Seat.Id);
        }

        private void InsertTestData(ApplicationDbContext context)
        {

            context.Add(new Cinema()
            {
                Id = 1,
                Name = "Moviehub",
                Address = "Chasseveld 15",
                PostalCode = "4811 DH",
                City = "Breda",
                Country = "Nederland",
                Latitude = 51.58955,
                Longitude = 4.78544,
                FacebookUrl = "www.facebook.com/moviehub",
                InstagramUrl = "www.instagram.com/moviehub",
                TwitterUrl = "www.twitter.com/moviehub",
                YoutubeUrl = "www.youtu.be/moviehub"
            });

            context.Add(new Hall()
            {
                Id = 1,
                CinemaId = 1,
                Name = "Hall 1",
                Capacity = 120,
                Has3d = true,
                DolbySurround = true,
                WheelchairAccess = true
            });

            context.Add(new Order()
            {
                Id = 1,
                ShowtimeId = 1,
            });

            context.Add(new Ticket()
            {
                Id = 1,
                OrderId = 1,
                Name = "testTicket",
                SeatId = 1
            });

            context.Add(new Showtime()
            {
                HallId = 1,
                MovieId = 1,
                StartAt = DateTime.Today.AddDays(1).AddHours(23)
            });

            context.Add(new Seat()
            {
                HallId = 1,
                SeatNumber = 1,
                RowNumber = 1,
            });

            context.Add(new Seat()
            {
                HallId = 1,
                SeatNumber = 2,
                RowNumber = 2,
            });

            context.SaveChanges();
        }
    }
}
