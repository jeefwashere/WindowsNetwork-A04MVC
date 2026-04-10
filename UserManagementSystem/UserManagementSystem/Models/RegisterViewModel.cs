using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string StreetName {  get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

    }
}
