using System.ComponentModel.DataAnnotations;

namespace WizytowkaFirmy.Models.DbModels
{
    public class WiadomoscEmail
    {
        [Key]
        public int Id { get; set; }
        public Klient Klient { get; set; }
        [Required(ErrorMessage = "Uzupełnij temat wiadomości.")]
        public string Temat { get; set; } = string.Empty;
        [Required(ErrorMessage = "Uzupełnij treść wiadomości.")]
        public string Tresc { get; set; } = string.Empty;
        public DateTime DataWyslania { get; set; }
    }
}
