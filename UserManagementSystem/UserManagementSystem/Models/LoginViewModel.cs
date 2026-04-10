using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string? ReturnUrl { get; set; } = null;
    }
}
