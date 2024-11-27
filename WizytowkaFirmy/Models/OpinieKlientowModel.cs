using System.ComponentModel.DataAnnotations;

namespace WizytowkaFirmy.Models
{
    public class OpinieKlientowModel
    {
        [Key]
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public int Ocena { get; set; }
        public string Komentarz { get; set; } = string.Empty;
        public DateTime DataWystawienia { get; set; }
        public bool JestUkryta { get; set; }

    }
}
