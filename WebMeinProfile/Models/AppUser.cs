using Microsoft.AspNetCore.Identity;

namespace WebMeinProfile.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Job { get; set; }
        public string Photo { get; set; }
    }
}
