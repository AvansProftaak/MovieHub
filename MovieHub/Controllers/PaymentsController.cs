using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.ViewModels;
using MovieHub.Controllers;
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

    public IActionResult ReceiveTicket(OrderViewModel orderViewModel)
    {
        // Here we call placeOrder from the ordersController
        OrdersController.PlaceOrder(_context);
        
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

    public async Task<IActionResult> Index(int orderId, OrderViewModel orderViewModel)
    {
        var payment = await _context.Payment
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
        orderViewModel.Payment = payment;
        return View(orderViewModel);
    }

    public async Task<string> getPaymentStatusCode(int orderId)
    {
        var payment = await _context.Payment
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (payment == null)
        {
            return "";
        }
        else
        {
            return payment.Status.ToString();
        }
    }

}