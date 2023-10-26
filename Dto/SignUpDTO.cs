using System.ComponentModel.DataAnnotations;

namespace BookstoreProject.Dto
{
    public class SignUpDTO
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
