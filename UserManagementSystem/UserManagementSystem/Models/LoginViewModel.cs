using System.ComponentModel.DataAnnotations;
/*
   * FILE : LoginViewModel.cs
   * PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
   * DESCRIPTION :set up for login page instance
   * 
   */
namespace UserManagementSystem.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public string? ReturnUrl { get; set; } = null;
    }
}
