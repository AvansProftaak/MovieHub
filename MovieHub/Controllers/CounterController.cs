using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using SkiaSharp;
using SkiaSharp.QrCode;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace MovieHub.Controllers
{
    public class CounterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CounterController(
            IWebHostEnvironment hostEnvironment, 
            ApplicationDbContext context
        )
        {
            _hostEnvironment= hostEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Coupon(string coupon)
        {
            if (coupon == "Voucher")
            {
                return View("Voucher");
            }
            if (coupon == "TenRides")
            {
                return View("TenRides");
            }
            return View("Subscription");
        }
        
        public IActionResult Payment(Voucher? voucher)
        {
            return View(voucher);
        }
        
        public IActionResult GetCoupon(string uuid)
        {
            var voucher = _context.Vouchers.FirstOrDefault(v => v.Barcode == uuid);
            return View(voucher);
        }
        
        public IActionResult PrintCoupon(string uuid)
        {
            var voucher = _context.Vouchers.FirstOrDefault(v => v.Barcode == uuid);
            if (voucher is not null)
            {
                return RedirectToAction("PrintVoucher", new { id = voucher.Id });
            }
            return View("Index");
        }

        public IActionResult PrintVoucher(int id)
        {
            var voucher = _context.Vouchers.FirstOrDefault(v => v.Id == id);

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
            
            // Dirs for storing finished HTML and PDF docs
            var FinishedPdfVoucherFolder = Path.Combine(wwwRootPath, @"ticket/FinishedPdfCoupon/");
            var FinishedHtmlVoucherFolder = Path.Combine(wwwRootPath, @"ticket/FinishedHtmlCoupon/");
            
            // Delete all finished tickets within the FinishedTickets folder
            DirectoryInfo di = new DirectoryInfo(FinishedPdfVoucherFolder);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
            
            // Delete all finished tickets within the FinishedHtmlTickets folder
            DirectoryInfo html_di = new DirectoryInfo(FinishedHtmlVoucherFolder);
            foreach (FileInfo file in html_di.GetFiles())
            {
                file.Delete(); 
            }
            foreach (DirectoryInfo dir in html_di.GetDirectories())
            {
                dir.Delete(true); 
            }

            var FinishedHtmlVoucherFile = Path.Combine(FinishedHtmlVoucherFolder, voucher.Id + ".html");

            if (voucher.Name == "Voucher")
            {
                // Get paths to HTML document
                var ExampleHtmlVoucherPath = Path.Combine(wwwRootPath, @"ticket/ExampleVoucherHtml.html");
            
                // Get ExampleTicketHtml
                var finishedHtmlVoucher = System.IO.File.ReadAllText(ExampleHtmlVoucherPath);

                var base64ImageRepresentation = CreateQr(voucher.Barcode);
            
                finishedHtmlVoucher = finishedHtmlVoucher
                    .Replace("#Amount", voucher.Price.ToString())
                    .Replace("QRCODE", base64ImageRepresentation)
                    .Replace("#UniqueId", voucher.Barcode);
                
                FinishedHtmlVoucherFile = Path.Combine(FinishedHtmlVoucherFolder, voucher.Id + ".html");
                
                // Save finished Html ticket
                System.IO.File.WriteAllText(FinishedHtmlVoucherFile, finishedHtmlVoucher);
            }
            if (voucher.Name == "TenRides")
            {
                // Get paths to HTML document
                var ExampleHtmlVoucherPath = Path.Combine(wwwRootPath, @"ticket/ExampleTenRidesHtml.html");
            
                // Get ExampleTicketHtml
                var finishedHtmlVoucher = System.IO.File.ReadAllText(ExampleHtmlVoucherPath);

                var base64ImageRepresentation = CreateQr(voucher.Barcode);
            
                finishedHtmlVoucher = finishedHtmlVoucher
                    .Replace("QRCODE", base64ImageRepresentation)
                    .Replace("#UniqueId", voucher.Barcode);
                
                FinishedHtmlVoucherFile = Path.Combine(FinishedHtmlVoucherFolder, voucher.Id + ".html");
                
                // Save finished Html ticket
                System.IO.File.WriteAllText(FinishedHtmlVoucherFile, finishedHtmlVoucher);
            }
            if (voucher.Name == "Subscription")
            {
                // Get paths to HTML document
                var ExampleHtmlVoucherPath = Path.Combine(wwwRootPath, @"ticket/ExampleSubscriptionHtml.html");
            
                // Get ExampleTicketHtml
                var finishedHtmlVoucher = System.IO.File.ReadAllText(ExampleHtmlVoucherPath);

                var base64ImageRepresentation = CreateQr(voucher.Barcode);
            
                finishedHtmlVoucher = finishedHtmlVoucher
                    .Replace("#FirstName", voucher.FirstName)
                    .Replace("#LastName", voucher.LastName)
                    .Replace("QRCODE", base64ImageRepresentation)
                    .Replace("#UniqueId", voucher.Barcode);
                
                FinishedHtmlVoucherFile = Path.Combine(FinishedHtmlVoucherFolder, voucher.Id + ".html");
                
                // Save finished Html ticket
                System.IO.File.WriteAllText(FinishedHtmlVoucherFile, finishedHtmlVoucher);
            }

            // Convert HTML to PDF
            PdfDocument document = htmlConverter.Convert(FinishedHtmlVoucherFile);

            // Create a filestream for the finished pdf ticket
            var FinishedPdfVoucherPath = Path.Combine(FinishedPdfVoucherFolder, voucher.Id + ".pdf");
            FileStream fileStream = new FileStream(FinishedPdfVoucherPath, FileMode.Create, FileAccess.ReadWrite);
                
            // Save and close files/streams
            document.Save(fileStream);
            document.Close(true);
            fileStream.Close();
            
            // Create byte file from Pdf
            var fileBytes = System.IO.File.ReadAllBytes(FinishedPdfVoucherPath);
            
            // Return Pdf for download
            return File(fileBytes, "application/pdf", "coupon.pdf");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Voucher(decimal amount)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Price", "Please enter a valid number.");
                return View();
            }
            if (amount < 5)
            {
                ModelState.AddModelError("Price", "Please enter an amount equal to or higher than &euro; 5.");
                return View();
            }
            Voucher voucher = new Voucher()
            {
                Name = "Voucher",
                Price = amount,
                paid = false,
                FirstName = "-",
                LastName = "-",
                Barcode = Guid.NewGuid().ToString()
            };
            _context.Vouchers.Add(voucher);
            _context.SaveChanges();
            
            return View("Payment", voucher);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TenRides()
        {
            Voucher voucher = new Voucher()
            {
                Name = "TenRides",
                Price = 70,
                paid = false,
                FirstName = "-",
                LastName = "-",
                Barcode = Guid.NewGuid().ToString()
            };
            _context.Vouchers.Add(voucher);
            _context.SaveChanges();
            
            return View("Payment", voucher);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscription(string FirstName, string LastName)
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                ModelState.AddModelError("FirstName", "Please enter a valid First Name");
                return View();
            }
            if (string.IsNullOrEmpty(LastName))
            {
                ModelState.AddModelError("FirstName", "Please enter a valid First Name");
                return View();
            }
            Voucher voucher = new Voucher()
            {
                Name = "Subscription",
                Price = 17,
                paid = false,
                FirstName = FirstName,
                LastName = LastName,
                Barcode = Guid.NewGuid().ToString()
            };
            _context.Vouchers.Add(voucher);
            _context.SaveChanges();
            
            return View("Payment", voucher);
        }
        
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(p => p.Id == id);
            if (voucher == null)
            {
                return NotFound();
            }
            
            voucher.paid = true;
            _context.Update(voucher);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetCoupon", new { uuid = voucher.Barcode });
        }

        public string CreateQr(string uuid)
        {
            var wwwRootPath = _hostEnvironment.WebRootPath;
            
            var content = uuid;
            using QRCodeGenerator generator = new QRCodeGenerator();

            ECCLevel level = ECCLevel.H;
            var qr = generator.CreateQrCode(content, level);

            SKImageInfo info = new SKImageInfo(512, 512);
            using SKSurface surface = SKSurface.Create(info);

            var canvas = surface.Canvas;
            canvas.Render(qr, SKRect.Create(512, 512), SKColors.White, SKColors.Black);

            using SKImage image = surface.Snapshot();
            using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);

            var subPath = Path.Combine(wwwRootPath, @"ticket/qr/");
            bool exists = System.IO.Directory.Exists(subPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(subPath);
            using FileStream stream = System.IO.File.OpenWrite(
                Path.Combine(subPath, @"qr-" + content + ".png")
            );
            data.SaveTo(stream);
            stream.Close();
            byte[] imageArray = System.IO.File.ReadAllBytes(
                Path.Combine(subPath, @"qr-" + content + ".png")
            );
            
            DirectoryInfo di = new DirectoryInfo(subPath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
            
            var base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }
    }
}