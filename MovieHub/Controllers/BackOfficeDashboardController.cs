using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using System.Net;
using System.Net.Mail;

namespace MovieHub.Controllers
{
    [Authorize(Roles = "Admin, BackOffice")]
    public class BackOfficeDashboardController : Controller
    {
        private readonly ILogger<BackOfficeDashboardController> _logger;
        private readonly ApplicationDbContext _context;

        public BackOfficeDashboardController(ILogger<BackOfficeDashboardController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendNewsLetter(string text)
        {
            
            var emails = await _context.Newsletter.ToListAsync();
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("moviehubnewsletter@gmail.com", "geheim1!"),
                EnableSsl = true,
            };
            foreach (var email in emails)
            {
                MailMessage message = new MailMessage("moviehubnewsletter@gmail.com", email.Email, "Moviehub Newsletter", CreateNewsLetter(text));
                message.IsBodyHtml = true;
                smtpClient.Send(message);
            }
            return RedirectToAction("Index");
        }

        private string CreateNewsLetter(string text)
        {
            string FilePath = Directory.GetCurrentDirectory() + "//wwwroot//email//index.html";
            StreamReader str = new StreamReader(FilePath);
            string content = str.ReadToEnd();
            str.Close();

            content = content.Replace("--blablabla--", text);
            return content;
        }
    }
}
