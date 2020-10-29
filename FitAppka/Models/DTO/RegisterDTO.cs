using System.ComponentModel.DataAnnotations;


namespace FitAppka.Models
{
    public class RegisterDTO
    {
        [MaxLength(20)]
        [Required(ErrorMessage = "Proszę podać imię")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [MaxLength(25)]
        [Required(ErrorMessage = "Proszę podać nazwisko")]
        [Display(Name = "Nazwisko")]
        public string SecondName { get; set; }

        [MaxLength(35)]
        [Required(ErrorMessage = "Proszę podać adres e-mail")]
        [Display(Name = "Adres e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(25)]
        [Required(ErrorMessage = "Proszę podać login")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Proszę podać hasło")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Proszę podać potwierdzenie hasła")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password",
            ErrorMessage = "Hasło różni się od potwierdzenia.")]
        public string ConfirmPassword { get; set; }
    }
}
