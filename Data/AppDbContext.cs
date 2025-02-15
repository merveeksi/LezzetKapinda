using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    
    
}