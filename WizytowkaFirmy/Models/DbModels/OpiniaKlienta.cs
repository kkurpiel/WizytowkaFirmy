using System.ComponentModel.DataAnnotations;

namespace WizytowkaFirmy.Models.DbModels
{
    public class OpiniaKlienta
    {
        [Key]
        public int Id { get; set; }
        public Klient Klient { get; set; }
        [Required(ErrorMessage = "Wystawienie oceny jest wymagane.")]
        public int Ocena { get; set; }
        public string? Komentarz { get; set; }
        public DateTime DataWystawienia { get; set; }
        public bool JestUkryta { get; set; }
    }
}
