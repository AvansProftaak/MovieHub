using Microsoft.AspNetCore.Mvc;
using MovieHub.Models;
using MovieHub.Services;

namespace MovieHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MailController : Controller
{
    private readonly IMailService mailService;
    public MailController(IMailService mailService)
    {
        this.mailService = mailService;
    }
    [HttpPost("send")]
    public async Task<IActionResult> SendMail([FromForm]MailRequest request)
    {
        try
        {
            await mailService.SendEmailAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            throw;
        }
            
    }
}