using Microsoft.AspNetCore.Mvc;
using ManageMadarsaCore.Data;
using ManageMadarsaCore.Models;
using System.Net;
using System.Net.Mail;

namespace ManageMadarsaCore.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public HomeController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IActionResult Index() => View();
    public IActionResult About() => View();
    public IActionResult Vision() => View();
    public IActionResult Contact() => View();
    public IActionResult Features() => View();
    public IActionResult Privacy() => View();

    [HttpPost]
    public async Task<IActionResult> SubmitContact([FromBody] ContactFormDTO model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.Email))
        {
            return BadRequest(new { success = false, message = "Please fill in all required fields." });
        }

        try
        {
            string fullName = string.IsNullOrWhiteSpace(model.LastName)
                ? model.FirstName.Trim()
                : $"{model.FirstName.Trim()} {model.LastName.Trim()}";

            var contactInfo = new ContactUsInfo
            {
                Title = "Mr/Ms",
                FirstName = fullName,
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber?.Trim(),
                Country = null,
                Message = string.IsNullOrWhiteSpace(model.Subject)
                    ? model.Message?.Trim()
                    : $"[Subject: {model.Subject.Trim()}] {model.Message?.Trim()}"
            };

            _context.tblcontactusinfo.Add(contactInfo);
            await _context.SaveChangesAsync();

            // Trigger background confirmation email to user
            _ = Task.Run(() => SendConfirmationEmail(model.Email.Trim(), fullName, model.Subject));

            return Ok(new { success = true, message = "Message sent successfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while saving your message: " + ex.Message });
        }
    }

    private void SendConfirmationEmail(string recipientEmail, string recipientName, string? subject)
    {
        try
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPortStr = _configuration["EmailSettings:SmtpPort"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"] ?? "ManageMadarsa Support";
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            if (string.IsNullOrWhiteSpace(smtpServer) || string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(senderPassword))
            {
                // Email settings not configured yet
                return;
            }

            int port = int.TryParse(smtpPortStr, out var p) ? p : 587;

            using var smtpClient = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = "We received your message - ManageMadarsa",
                Body = $@"
                    <div style='font-family: Arial, sans-serif; padding: 25px; color: #222; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px;'>
                        <div style='text-align: center; padding-bottom: 20px; border-bottom: 2px solid #a2ae40;'>
                            <h2 style='color: #2b5741; margin: 0;'>ManageMadarsa</h2>
                            <p style='color: #777; font-size: 0.9rem; margin-top: 5px;'>Support & Inquiry Confirmation</p>
                        </div>
                        <div style='padding: 20px 0;'>
                            <p style='font-size: 1.1rem;'>Dear <strong>{WebUtility.HtmlEncode(recipientName)}</strong>,</p>
                            <p>Thank you for reaching out to us! We have successfully received your message regarding <strong>'{WebUtility.HtmlEncode(subject ?? "Inquiry")}'</strong>.</p>
                            <p>Our team is reviewing your details and will respond to you within 24 hours.</p>
                        </div>
                        <div style='background-color: #f9f9f9; padding: 15px; border-radius: 6px; font-size: 0.9rem; color: #555;'>
                            <strong>Need immediate assistance?</strong><br>
                            Call our helpline: +91 852 1995869 | Email: enquiry@managemadarsa.com
                        </div>
                        <div style='text-align: center; margin-top: 25px; font-size: 0.8rem; color: #aaa;'>
                            &copy; ManageMadarsa. All rights reserved.
                        </div>
                    </div>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(recipientEmail);
            smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Email Error] Could not send confirmation email: {ex.Message}");
        }
    }
}
