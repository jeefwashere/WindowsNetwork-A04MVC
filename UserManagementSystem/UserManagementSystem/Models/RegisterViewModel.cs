using System.ComponentModel.DataAnnotations;
/*
   * FILE : RegisterView.cs
   * PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
   * DESCRIPTION :set up for RegisterViewModel 
   * 
   */
namespace UserManagementSystem.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string StreetName {  get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PostalCode { get; set; }

    }
}
