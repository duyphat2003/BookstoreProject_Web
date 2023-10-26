using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace BookstoreProject.Dto
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string Account { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }
    }
}
