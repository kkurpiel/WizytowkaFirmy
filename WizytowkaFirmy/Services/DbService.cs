using Microsoft.EntityFrameworkCore;
using NLog;
using System.Transactions;
using WizytowkaFirmy.Data;
using WizytowkaFirmy.Models;
using WizytowkaFirmy.Models.DbModels;

namespace WizytowkaFirmy.Services
{
    public class DbService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly AppDbContext context;

        public DbService(DbContextOptions<AppDbContext> options)
        {
            this.context = new AppDbContext(options);
        }

        /// <summary>
        /// Dodaj nową wiadomość e-mail do bazy danych
        /// </summary>
        /// <param name="wiadomosc"></param>
        /// <returns></returns>
        public async Task DodajWiadomosc(WiadomoscEmail wiadomosc)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var klient = await context.Klient.FirstOrDefaultAsync(x => x.Email == wiadomosc.Klient.Email);
                    if (klient == null)
                    {
                        klient = new Klient
                        {
                            Email = wiadomosc.Klient.Email
                        };
                        await context.AddAsync(klient);
                        logger.Info($"Dodano nowego klienta {klient.Email}");
                    }
                    var nowaWiadomosc = new WiadomoscEmail
                    {
                        Klient = klient,
                        Temat = wiadomosc.Temat,
                        Tresc = wiadomosc.Tresc,
                        DataWyslania = DateTime.Now
                    };
                    await context.AddAsync(nowaWiadomosc);
                    logger.Info("Dodano nową wiadomość");
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// Dodaj nową opinię klienta oraz nowego klienta do bazy danych, jeśli ten jeszcze w niej nie istnieje
        /// </summary>
        /// <param name="opiniaKlienta"></param>
        /// <returns></returns>
        public async Task DodajOpinie(OpiniaKlienta opiniaKlienta)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var klient = await context.Klient.FirstOrDefaultAsync(x => x.Email == opiniaKlienta.Klient.Email);
                    if (klient == null)
                    {
                        klient = new Klient
                        {
                            Email = opiniaKlienta.Klient.Email
                        };
                        await context.AddAsync(klient);
                        logger.Info($"Dodano nowego klienta {klient.Email}");
                    }
                    var nowaOpinia = new OpiniaKlienta
                    {
                        Klient = klient,
                        Ocena = opiniaKlienta.Ocena,
                        Komentarz = opiniaKlienta.Komentarz,
                        DataWystawienia = DateTime.Now,
                        JestUkryta = false
                    };
                    await context.AddAsync(nowaOpinia);
                    logger.Info($"Dodano nową opinię");
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// Pobierz listę wszystkich opinii, które nie zostały ukryte
        /// </summary>
        /// <returns></returns>
        public async Task<List<OpiniaKlienta>> OpinieKlientow()
        {
            try
            {
                var opinie = await context.OpiniaKlienta.Include(x => x.Klient).Where(x => x.JestUkryta == false).OrderByDescending(x => x.DataWystawienia).ToListAsync();
                return opinie;
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return new List<OpiniaKlienta>();
            }
        }

        public async Task UkryjOpinie(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var opinia = await context.OpiniaKlienta.FirstOrDefaultAsync(x => x.Id == id);
                    if (opinia != null)
                    {
                        opinia.JestUkryta = true;
                        await context.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    transaction.Rollback();
                }
            }
        }
    }
}
