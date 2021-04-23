namespace FitnessApp.Models.DTO
{
    public class LoginDTO
    {
        public string LoginOrEmail { get; set; }
        public string Password { get; set; }
        public bool? WasModalOpened { get; set; }
    }
}
