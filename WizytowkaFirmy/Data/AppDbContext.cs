using Microsoft.EntityFrameworkCore;
using WizytowkaFirmy.Models.DbModels;

namespace WizytowkaFirmy.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Klient> Klient { get; set; }
        public DbSet<OpiniaKlienta> OpiniaKlienta { get; set; }
        public DbSet<WiadomoscEmail> WiadomoscEmail { get; set; }
    }
}
