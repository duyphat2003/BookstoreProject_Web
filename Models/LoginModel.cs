using System.ComponentModel.DataAnnotations;

namespace BookstoreProject.Models
{
    public class LoginModel:Account
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
