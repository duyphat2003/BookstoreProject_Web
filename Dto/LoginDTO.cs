using System.ComponentModel.DataAnnotations;
using BookstoreProject.Models;

namespace BookstoreProject.Dto
{
    public class LoginDTO : Account
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }

        
    }
}
