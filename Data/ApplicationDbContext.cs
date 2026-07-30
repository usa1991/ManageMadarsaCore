using Microsoft.EntityFrameworkCore;
using ManageMadarsaCore.Models;

namespace ManageMadarsaCore.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContactUsInfo> tblcontactusinfo { get; set; } = null!;
}
