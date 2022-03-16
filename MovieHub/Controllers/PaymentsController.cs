using System.Net;
using System.Reflection;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.ViewModels;
using MovieHub.Controllers;
using MovieHub.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace MovieHub.Controllers;

public class PaymentsController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ApplicationDbContext _context;
    public PaymentsController(IWebHostEnvironment hostEnvironment, ApplicationDbContext context)
    {
        _hostEnvironment= hostEnvironment;
        _context = context;
    }

    public IActionResult ReceiveTicket()
    {
        // TODO: Receive information about the payment
        
        //Get wwwroot path information
        var wwwRootPath = _hostEnvironment.WebRootPath;

        //Initialize HTML to PDF converter with Blink rendering engine
        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
        BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
        
        //Check for current OS and set the BlinkBinaries folder path
        if (OperatingSystem.IsMacOS())
        {
            blinkConverterSettings.BlinkPath = @"Binaries/BlinkBinariesMac/";
            Console.WriteLine("We're on macOS!");
        }else if (OperatingSystem.IsLinux())
        {
            blinkConverterSettings.BlinkPath = @"Binaries/BlinkBinariesLinux/";
            Console.WriteLine("We're on Linux!");
        } else if (OperatingSystem.IsWindows())
        {
            blinkConverterSettings.BlinkPath = @"Binaries/BlinkBinariesWindows/";
            Console.WriteLine("We're on Windows!");
        }

        //Assign Blink converter settings to HTML converter
        htmlConverter.ConverterSettings = blinkConverterSettings;

        // Get paths to PDF and HTML documents
        var ExampleHtmlTicketPath = Path.Combine(wwwRootPath, @"ticket/ExampleTicketHtml.html");
        var FinishedHtmlTicketPath = Path.Combine(wwwRootPath, @"ticket/FinishedTicketHtml.html");
        var FinishedPdfTicketPath = Path.Combine(wwwRootPath, @"ticket/TicketPdf.pdf");

        //Remove existing finished tickets before creating new ones
        if (System.IO.File.Exists(FinishedPdfTicketPath))
        {
            System.IO.File.Delete(FinishedPdfTicketPath);
        }
        
        if (System.IO.File.Exists(FinishedHtmlTicketPath))
        {
            System.IO.File.Delete(FinishedHtmlTicketPath);
        }
        
        //Fetch ExampleTicketHtml
        var finishedHtmlTicket = System.IO.File.ReadAllText(ExampleHtmlTicketPath);
        
        //Replace all values
        finishedHtmlTicket = finishedHtmlTicket
            .Replace("#MovieName", "Two Girls one Cup")
            .Replace("#Person", "John Doe")
            .Replace("#Seat", "7A")
            .Replace("#Time", "12:00")
            .Replace("QRCODE", "Dummy QR");

        //Save finished Html ticket
        System.IO.File.WriteAllText(FinishedHtmlTicketPath, finishedHtmlTicket);

        //Convert HTML to PDF
        PdfDocument document = htmlConverter.Convert(FinishedHtmlTicketPath);
        FileStream fileStream = new FileStream(FinishedPdfTicketPath, FileMode.CreateNew, FileAccess.ReadWrite);

        //Save and close the PDF document 
        document.Save(fileStream);
        document.Close(true);
        fileStream.Close();
        
        //Create byte file from Pdf
        var fileBytes = System.IO.File.ReadAllBytes(FinishedPdfTicketPath);
        
        //Return Pdf for download
        return File(fileBytes, "application/pdf", "ticket.pdf");
    }

    public async Task<IActionResult> Index( Dictionary<string,string> json)
    {
        Console.Write(json);
        OrderData orderData = JsonConvert.DeserializeObject<OrderData>(json["orderData"]);
        Console.Write(orderData);

        int movieId = orderData.movieId;
        int showtimeId = orderData.showtimeId;
        int userId = 1;
        Dictionary<string, int> ticketTypesSelected = orderData.ticketTypes;
        Dictionary<string, int> cateringPackagesSelected = orderData.cateringPackages;
        Dictionary<string, string> seatsSelected = orderData.seats;
        
        Showtime showtime = _context.Showtime
            .Where(s => (s.Id >= showtimeId)).FirstOrDefault();
        //int orderId = OrdersController.PlaceOrder(_context);
        User user = _context.User
            .Where(u => (u.Id >= userId)).FirstOrDefault();
        //int orderId = OrdersController.PlaceOrder(_context);

        Order order = new Order();
        order.UserId = userId;
        order.Showtime = showtime;
        order.ShowtimeId = showtimeId;
        order.User = user;
        
        

        _context.Order.Add(order);
        await _context.SaveChangesAsync();
        

        
        Movie movie = OrdersController.GetMovie(movieId, _context);
        List<Tickettype> tickettypes = OrdersController.TicketTypes(showtime.MovieId, _context);
        List<CateringPackage> cateringPackages = OrdersController.GetCateringPackages(_context);


        int counter = 0;
        foreach (var key in ticketTypesSelected.Keys)
        {
            int i = 0;
            Tickettype tickettype = tickettypes[counter];

            while (i <= ticketTypesSelected[key])
            {
                Ticket ticket = new Ticket();
                ticket.Barcode = 123;
                ticket.OrderId = order.Id;
                ticket.Name = tickettype.Name;
                ticket.Price = tickettype.Price;
                ticket.SeatId = 22;

                _context.Ticket.Add(ticket);
                await _context.SaveChangesAsync();
                i++;
            }
            
            //ticket.Name 
            counter += 1;
        }




            var payment = await _context.Payment
            .FirstOrDefaultAsync(p => p.OrderId == order.Id);
        return View(payment);
    }

    public async Task<IActionResult> UpdateStatus(int orderId, int status)
    {
        var payment = await _context.Payment
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (payment == null)
        {
            return NotFound();
        } else
        {
            switch(status)
            {
                case 1:
                    payment.Status = Models.StatusEnum.Pending;
                    break;
                case 2:
                    payment.Status = Models.StatusEnum.Paid;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(status));
            }
            _context.Update(payment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", new { orderId = orderId });
    }
}