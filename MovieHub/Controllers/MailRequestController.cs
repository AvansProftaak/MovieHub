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
using MovieHub.Services;

namespace MovieHub.Controllers
{
    public class MailRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailService mailService;

        public MailRequestController(ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            this.mailService = mailService;
        }

        // GET: MailRequest
        public async Task<IActionResult> Index()
        {
            return View(await _context.MailRequest.ToListAsync());
        }

        // GET: MailRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailRequest = await _context.MailRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailRequest == null)
            {
                return NotFound();
            }

            return View(mailRequest);
        }

        // GET: MailRequest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MailRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ToEmail,Subject,Body")] MailRequest mailRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mailRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(mailRequest);
        }

        // GET: MailRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailRequest = await _context.MailRequest.FindAsync(id);
            if (mailRequest == null)
            {
                return NotFound();
            }

            return View(mailRequest);
        }

        // POST: MailRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ToEmail,Subject,Body")] MailRequest mailRequest)
        {
            if (id != mailRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mailRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailRequestExists(mailRequest.Id))
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

            return View(mailRequest);
        }

        // GET: MailRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailRequest = await _context.MailRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailRequest == null)
            {
                return NotFound();
            }

            return View(mailRequest);
        }

        // POST: MailRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mailRequest = await _context.MailRequest.FindAsync(id);
            _context.MailRequest.Remove(mailRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MailRequestExists(int id)
        {
            return _context.MailRequest.Any(e => e.Id == id);
        }


        public async Task<IActionResult> SendNewsletter(int id)
        {
            var mailRequestSaved = await _context.MailRequest.FindAsync(id);

            var applicationUsers = _context.Users
                .Where(u => (u.AcceptedNewsletter.Equals(true)));
            
            var newsletterSubscriptions = _context.Newsletter;
            
            var newsletterSubscriptionsList = new List<string>();

            foreach (var email in newsletterSubscriptions)
            {
                newsletterSubscriptionsList.Add(email.Email);
            }

            foreach (var applicationUser in applicationUsers)
            {
                newsletterSubscriptionsList.Add(applicationUser.Email);
            }

            var discriptionsDistinct = newsletterSubscriptionsList.Distinct();
                        

            foreach (var email in discriptionsDistinct)
            {
                MailRequest mailRequest = new()
                {
                    ToEmail = email,
                    Subject = mailRequestSaved.Subject,
                    Body = mailRequestSaved.Body,
                };
                
                await mailService.SendNewsletterAsync(mailRequest);

            } TempData["message"] = "Newsletter has been send!";
                return RedirectToAction(nameof(Index));
        }
    }
}

