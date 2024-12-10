using System.ComponentModel.DataAnnotations;

namespace WizytowkaFirmy.Models.DbModels
{
    public class Klient
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Uzupełnij adres e-mail.")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres e-mail.")]
        public string Email { get; set; } = string.Empty;
    }
}
