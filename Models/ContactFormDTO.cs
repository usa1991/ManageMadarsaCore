namespace ManageMadarsaCore.Models;

public class ContactFormDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
}
