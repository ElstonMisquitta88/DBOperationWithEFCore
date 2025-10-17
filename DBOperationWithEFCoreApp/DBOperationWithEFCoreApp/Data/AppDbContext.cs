using Microsoft.EntityFrameworkCore;
namespace DBOperationWithEFCoreApp.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }
}
