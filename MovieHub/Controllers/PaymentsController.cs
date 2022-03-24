using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using Newtonsoft.Json;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

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

    public IActionResult ReceiveTicket(int orderId)
    {
        var order = _context.Order.FirstOrDefault(o => o.Id == orderId);
        var showTime = _context.Showtime.FirstOrDefault(s => s.Id == order.ShowtimeId);
        var hall = _context.Hall.FirstOrDefault(h => h.Id == showTime.HallId);
        var movie = _context.Movie.FirstOrDefault(m => m.Id == showTime.MovieId);

        // Initialize HTML to PDF converter with Blink rendering engine
        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
        BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
        
        // Check for current OS and set the BlinkBinaries folder path
        if (OperatingSystem.IsMacOS())
        {
            blinkConverterSettings.BlinkPath = @"Binaries/BlinkBinariesMac/";
            Console.WriteLine("We're on macOS!");
        } else if (OperatingSystem.IsLinux())
        {
            blinkConverterSettings.BlinkPath = @"Binaries/BlinkBinariesLinux/";
            Console.WriteLine("We're on Linux!");
        } else if (OperatingSystem.IsWindows())
        {
            blinkConverterSettings.BlinkPath = @"Binaries/BlinkBinariesWindows/";
            Console.WriteLine("We're on Windows!");
        }

        // Assign Blink converter settings to HTML converter
        htmlConverter.ConverterSettings = blinkConverterSettings;
        
        // Get wwwroot path information
        var wwwRootPath = _hostEnvironment.WebRootPath;

        // Get paths to PDF and HTML documents
        var ExampleHtmlTicketPath = Path.Combine(wwwRootPath, @"ticket/ExampleTicketHtml.html");
        var ExampleHtmlArrangementPath = Path.Combine(wwwRootPath, @"ticket/ExampleArrangementHtml.html");
        var FinishedPdfTicketsPath = Path.Combine(wwwRootPath, @"ticket/TicketsPdf.pdf");
        
        // Dirs for storing finished HTML and PDF docs
        var FinishedPdfTicketFolder = Path.Combine(wwwRootPath, @"ticket/FinishedPdfTickets/");
        var FinishedHtmlTicketFolder = Path.Combine(wwwRootPath, @"ticket/FinishedHtmlTickets/");
        
        // Delete all finished tickets within the FinishedTickets folder
        DirectoryInfo di = new DirectoryInfo(FinishedPdfTicketFolder);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete(); 
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true); 
        }
        
        // Delete all finished tickets within the FinishedHtmlTickets folder
        DirectoryInfo html_di = new DirectoryInfo(FinishedHtmlTicketFolder);
        foreach (FileInfo file in html_di.GetFiles())
        {
            file.Delete(); 
        }
        foreach (DirectoryInfo dir in html_di.GetDirectories())
        {
            dir.Delete(true); 
        }

        string[] arrangements = {"Popcorn", "Popcorn and Soda", "VIP", "Children's party"};

        var tickets = _context.Ticket.Where(t => t.OrderId == orderId).ToList();
        foreach (var ticket in tickets)
        {
            if (arrangements.Contains(ticket.Name))
            {
                // Get ExampleTicketHtml
                var finishedHtmlArrangement = System.IO.File.ReadAllText(ExampleHtmlArrangementPath);

                finishedHtmlArrangement = finishedHtmlArrangement
                    .Replace("#Type", ticket.Name);

                var FinishedHtmlArrangementFile = Path.Combine(FinishedHtmlTicketFolder, ticket.Id + ".html");
                
                // Save finished Html ticket
                System.IO.File.WriteAllText(FinishedHtmlArrangementFile, finishedHtmlArrangement);

                // Convert HTML to PDF
                PdfDocument document = htmlConverter.Convert(FinishedHtmlArrangementFile);

                // Create a filestream for the finished pdf ticket
                var FinishedPdfTicketPath = Path.Combine(FinishedPdfTicketFolder, ticket.Id + ".pdf");
                FileStream fileStream = new FileStream(FinishedPdfTicketPath, FileMode.Create, FileAccess.ReadWrite);
            
                // Save and close files/streams
                document.Save(fileStream);
                document.Close(true);
                fileStream.Close();
            }
            else
            {
                // Get ExampleTicketHtml
                var finishedHtmlTicket = System.IO.File.ReadAllText(ExampleHtmlTicketPath);

                // Get seat && row for current ticket
                var seat = _context.Seat.FirstOrDefault(s => s.Id == ticket.SeatId);

                finishedHtmlTicket = finishedHtmlTicket
                    .Replace("#MovieName", movie.Title)
                    .Replace("#Type", ticket.Name)
                    .Replace("#HallNumber", hall.Name)
                    .Replace("#Seat", seat.SeatNumber.ToString())
                    .Replace("#Row", seat.RowNumber.ToString())
                    .Replace("#Time", showTime.StartAt.TimeOfDay.ToString())
                    .Replace("#Date", showTime.StartAt.ToShortDateString())
                    .Replace("QRCODE", "Dummy QR");
            
                var FinishedHtmlTicketFile = Path.Combine(FinishedHtmlTicketFolder, ticket.Id + ".html");
            
                // Save finished Html ticket
                System.IO.File.WriteAllText(FinishedHtmlTicketFile, finishedHtmlTicket);

                // Convert HTML to PDF
                PdfDocument document = htmlConverter.Convert(FinishedHtmlTicketFile);

                // Create a filestream for the finished pdf ticket
                var FinishedPdfTicketPath = Path.Combine(FinishedPdfTicketFolder, ticket.Id + ".pdf");
                FileStream fileStream = new FileStream(FinishedPdfTicketPath, FileMode.Create, FileAccess.ReadWrite);
            
                // Save and close files/streams
                document.Save(fileStream);
                document.Close(true);
                fileStream.Close();
            }

        }
        
        // Get the folder path into DirectoryInfo
        DirectoryInfo directoryInfo = new DirectoryInfo(FinishedPdfTicketFolder);
 
        // Get the PDF files in folder path into FileInfo
        FileInfo[] files = directoryInfo.GetFiles("*.pdf");
 
        // Create a new PDF document 
        PdfDocument pdfTicketCombined = new PdfDocument();
 
        // Set enable memory optimization as true 
        pdfTicketCombined.EnableMemoryOptimization = true;
        
        // Open file stream for appending
        FileStream finishedPdfs = new FileStream(FinishedPdfTicketsPath, FileMode.Create, FileAccess.ReadWrite);
        
        // Loop over all the files in the FinishedTickets dir
        foreach (FileInfo file in files)
        {
            // Load the PDF document 
            FileStream fileStream = new FileStream(file.FullName, FileMode.Open);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileStream);
 
            // Merge PDF file
            PdfDocumentBase.Merge(pdfTicketCombined, loadedDocument);
 
            // Close the existing PDF document 
            loadedDocument.Close(true);
        }
 
        // Save the PDF document
        pdfTicketCombined.Save(finishedPdfs);
 
        // Close the instance of PdfDocument
        pdfTicketCombined.Close(true);
        finishedPdfs.Close();
        // Create byte file from Pdf
        var fileBytes = System.IO.File.ReadAllBytes(FinishedPdfTicketsPath);
        
        // Return Pdf for download
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

        var user = _context.User.FirstOrDefault(u => (u.Id >= userId))!;

        var order = new Order
        {
            UserId = userId,
            Showtime = showtime,
            ShowtimeId = showtimeId,
            User = user
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