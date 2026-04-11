using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models
{
    public class AddViewModel
    {
        [Required]
        public string ItemName { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
