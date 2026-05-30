using Microsoft.EntityFrameworkCore;

namespace Nutria.Infrastructure;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}