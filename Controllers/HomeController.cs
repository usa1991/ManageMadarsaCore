using Microsoft.AspNetCore.Mvc;
using ManageMadarsaCore.Data;
using ManageMadarsaCore.Models;

namespace ManageMadarsaCore.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
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

            return Ok(new { success = true, message = "Message sent successfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while saving your message: " + ex.Message });
        }
    }
}
