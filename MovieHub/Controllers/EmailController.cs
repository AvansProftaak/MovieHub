#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public async Task<ActionResult> Create([Bind("Subject,Content")] Email email)
        {
            var emailAddress = "newsletter.moviehub@gmail.com";
            var password = "P@ssword123!";

            string FilePath = Directory.GetCurrentDirectory() + "//wwwroot//templates//Moviehub_Newsletter_Template.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            
            MailText = MailText.Replace("[SUBJECT]", email.Subject).Replace("[CONTENT]", email.Content);

            
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

            var styledContent = new BodyBuilder();
            styledContent.HtmlBody = MailText;

            message.Subject = email.Subject;
            message.Body = styledContent.ToMessageBody();
            
            


            // user mailkit smtp client
            var client = new SmtpClient();

            try
            {
                //due to ssl not trustig gmail anymore ( on sunday 4/10/22) quick fix
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => { return true; };
                
                
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
            
            _context.Email.Add(email); 
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));

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

