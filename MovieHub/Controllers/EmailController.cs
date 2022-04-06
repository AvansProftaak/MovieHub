#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;

using System;
using System.Collections.Generic;
using System.Text;
using EASendMail;
using SmtpClient = EASendMail.SmtpClient; //add EASendMail namespace

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

        // POST: Email/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content")] Email email)
        {
            if (ModelState.IsValid)
            {
                _context.Add(email);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(email);
        }

        // GET: Email/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.Email.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }
            return View(email);
        }

        // POST: Email/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content")] Email email)
        {
            if (id != email.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(email);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailExists(email.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(email);
        }*/

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
        
        
        
        
        
        
        
        
        
        
        [HttpPost]  
        public ActionResult Create(string receiver, string subject, string message) {  
            /*try {  
                //if (ModelState.IsValid) {  
                    var senderEmail = new MailAddress("bgrootoonk@pepisandbox.com", "Bart");  
                    var receiverEmail = new MailAddress("bart-grootoonk@hotmail.com", "bart");  
                    var password = "P@ssword123!";  
                    var sub = "test";  
                    var body = "test";  
                    var smtp = new SmtpClient {  
                        Host = "smtp.netcorecloud.net",  
                        Port = 587,  
                        EnableSsl = true,  
                        DeliveryMethod = SmtpDeliveryMethod.Network,  
                        UseDefaultCredentials = false,  
                        Credentials = new NetworkCredential("bgrootoonk@pepisandbox.com", password)  
                    };  
                    using(var mess = new MailMessage(senderEmail, receiverEmail) {  
                              Subject = "test",  
                              Body = "test"  
                          }) {  
                        smtp.Send(mess);  
                    }

                    return View();
                //}  
            } catch (Exception) {  
                ViewBag.Error = "Some Error";  
            }  
            return View();*/
            
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                // Set sender email address, please change it to yours
                oMail.From = "test@emailarchitect.net";
                // Set recipient email address, please change it to yours
                oMail.To = "bart-grootoonk@hotmail.com";

                // Set email subject
                oMail.Subject = "direct email sent from c# project";
                // Set email body
                oMail.TextBody = "this is a test email sent from c# project directly";

                // Set SMTP server address to "".
                SmtpServer oServer = new SmtpServer("");

                // Do not set user authentication
                // Do not set SSL connection

                Console.WriteLine("start to send email directly ...");

                SmtpClient oSmtp = new SmtpClient();
                oSmtp.SendMail(oServer, oMail);

                Console.WriteLine("email was sent successfully!");
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            }
            return View();
        }

        }  
    }

