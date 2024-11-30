using System.ComponentModel.DataAnnotations;

namespace WizytowkaFirmy.Models
{
    public class NapiszDoNasModel
    {
        [Required(ErrorMessage ="Uzupełnij adres e-mail, na który mamy Ci odpowiedzieć.")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage ="Uzupełnij temat wiadomości.")]
        public string Temat { get; set; } = string.Empty;
        [Required(ErrorMessage ="Uzupełnij treść wiadomości.")]
        public string Tresc { get; set; } = string.Empty;
    }
}
