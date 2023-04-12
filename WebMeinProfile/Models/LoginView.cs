using System.ComponentModel.DataAnnotations;

namespace WebMeinProfile.Models
{
    public class LoginView
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
