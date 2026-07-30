using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageMadarsaCore.Models;

[Table("tblcontactusinfo")]
public class ContactUsInfo
{
    [Key]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Country { get; set; }

    public string? Message { get; set; }
}
