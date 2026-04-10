using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string HashedPassword { get; set; } = string.Empty;
        [Required]
        public string? StreetAddress {  get; set; } = string.Empty;
        [Required]
        public string? City { get; set; } = string.Empty;
        [Required]
        public string? Province { get; set; } = string.Empty;
        [Required]
        public string? Country { get; set; } = string.Empty;
        [Required]
        public string? PostalCode { get; set; } = string.Empty;
        public List<UserItem> Items { get; set; } = new List<UserItem>();
    }
}
