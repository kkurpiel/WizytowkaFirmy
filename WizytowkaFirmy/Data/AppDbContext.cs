using Microsoft.EntityFrameworkCore;
using WizytowkaFirmy.Models;

namespace WizytowkaFirmy.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<OpinieKlientowModel> OpinieKlientow { get; set; }
    }
}
