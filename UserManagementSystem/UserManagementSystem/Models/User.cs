using System.ComponentModel.DataAnnotations;
/*
   * FILE : UserModel.cs
   * PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
   * DESCRIPTION :set up for User<pde;
   * 
   */
namespace UserManagementSystem.Models
{
    public class User
    {
        //required means have to fill
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        //this will use on fronter to show
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
