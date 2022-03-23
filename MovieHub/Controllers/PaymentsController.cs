using System.Net;
using System.Reflection;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly UserManager<IdentityUser> _userManager;
    public PaymentsController(IWebHostEnvironment hostEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _hostEnvironment= hostEnvironment;
        _context = context;
        _userManager = userManager;
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
        OrderData? orderData = JsonConvert.DeserializeObject<OrderData>(json["orderData"]);

        int movieId = orderData.movieId;
        int showtimeId = orderData.showtimeId;
        int userId = 1;
        Dictionary<string, int> ticketTypesSelected = orderData.ticketTypes;
        Dictionary<string, int> cateringPackagesSelected = orderData.cateringPackages;

        Showtime? showtime = _context.Showtime!.FirstOrDefault(s => (s.Id >= showtimeId));


        List<List<string>> seatsSelected = orderData.seats;

        List<int> seatIds = new List<int>();
        foreach (List<string> seat in seatsSelected)
        {
            int? seatId = _context.Seat
                .Where(s => s.RowNumber.Equals(Int32.Parse(seat[0])))
                .Where(s => s.SeatNumber.Equals(Int32.Parse(seat[1])))
                .ToList().FirstOrDefault()?.Id;
            if (seatId.HasValue)
            {
                seatIds.Add((int) seatId);
            }
        }

        var user = _userManager.GetUserAsync(HttpContext.User);

        var order = new Order
        {
            UserId = _userManager.GetUserId(HttpContext.User),
            Showtime = showtime,
            ShowtimeId = showtimeId,
            User = await user
        };

        Insert(_context, order);
        /*_context.Order.Add(order);
        await _context.SaveChangesAsync();*/

        var movie = OrdersController.GetMovie(movieId, _context);
        var ticketTypesPrices = OrdersController.CalculationTicketTypes(showtime.MovieId, _context);
        var ticketTypesNames = OrdersController.ReturnTicketNames(showtime.MovieId, _context);
        var cateringPackages = OrdersController.GetCateringPackages(_context);
        var rd = new Random();


        var counter = 0;
        var seatsCounter = 0;
        foreach (var key in ticketTypesSelected.Keys)
        {
            var ticketId = Convert.ToInt32(key);
            var i = 1;

            var loopCount = (int) ticketTypesSelected[key];
            
            while (i <= loopCount )
            {
                var ticket = new Ticket
                {
                    Barcode = rd.Next(134909324, 912453657),
                    OrderId = order.Id,
                    Name = ticketTypesNames[ticketId],
                    Price = ticketTypesPrices[ticketId],
                    SeatId = seatIds[seatsCounter]
                };

                Insert(_context, ticket);
                seatsCounter++;
                i++;
            }

            counter += 1;
        }

        counter = 0;
        
        foreach (var key in cateringPackagesSelected.Keys)
        {
            var i = 1;
            var cateringPackage = cateringPackages[counter];

            while (i <= cateringPackagesSelected[key])
            {
                var ticket = new Ticket
                {
                    Barcode = rd.Next(134909324, 912453657),
                    OrderId = order.Id,
                    Name = cateringPackage.Name,
                    Price = cateringPackage.Price,
                    SeatId = null
                };


                Insert(_context, ticket);
                i++;
            }
            
            //ticket.Name 
            counter += 1;
        }





        var payment = await _context.Payment
        .FirstOrDefaultAsync(p => p.OrderId == order.Id);
        return View(order.Id);
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
        return RedirectToAction("GetPayment", new { orderId = orderId });
    }

    public async Task<IActionResult> CreatePayment(int orderId, int paymentMethodId)
    {
        Payment payment = new Payment();
        payment.Status = StatusEnum.Open;
        payment.OrderId = orderId;
        payment.PaymentMethodId = paymentMethodId;

        _context.Payment.Add(payment);
        await _context.SaveChangesAsync();
        var getPayment = await _context.Payment
    .FirstOrDefaultAsync(p => p.OrderId == orderId);
        return View(getPayment);
    }

    public async Task<IActionResult> GetPayment(int orderId)
    {
        var getPayment = await _context.Payment
    .FirstOrDefaultAsync(p => p.OrderId == orderId);
        return View(getPayment);
    }
    
    public static void Insert(DbContext context, object entity)
    {
        context.Add(entity);
        context.SaveChanges();
    }
}