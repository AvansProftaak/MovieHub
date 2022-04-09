#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;


namespace MovieHub.Controllers
{
    public class EmailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Email
        public async Task<IActionResult> Index()
        {
            return View(await _context.Email.ToListAsync());
        }

        // GET: Email/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email
                .FirstOrDefaultAsync(m => m.Id == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // GET: Email/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // this bit of code will be used to send emails from the create page
        [HttpPost]
        public ActionResult Create([Bind("Subject,Content")] Email email)
        {
            var emailAddress = "newsletter.moviehub@gmail.com";
            var password = "P@ssword123!";

            // create mime object of message to fill
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Bart", emailAddress));
            
            List<MailboxAddress> mailList = new List<MailboxAddress>();

            List<Newsletter> allNewsletterSubscribers = _context.Newsletter.ToList();
            foreach (Newsletter subscribed in allNewsletterSubscribers)
            {
                var mailAddress = subscribed.Email;
                mailList.Add(new MailboxAddress("Subscriber", mailAddress));
            }
            message.To.AddRange(mailList);

            message.Subject = email.Subject;
            message.Body = new TextPart("html")
            {
                Text = @email.Content
            };

            // user mailkit smtp client
            var client = new SmtpClient();

            try
            {
                // connect to gmail
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(emailAddress, password);
                client.Send(message);

                Console.WriteLine("email send");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }

            return View();

        }

        // GET: Email/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email
                .FirstOrDefaultAsync(m => m.Id == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // POST: Email/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var email = await _context.Email.FindAsync(id);
            _context.Email.Remove(email);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailExists(int id)
        {
            return _context.Email.Any(e => e.Id == id);
        }
        

    }
}
