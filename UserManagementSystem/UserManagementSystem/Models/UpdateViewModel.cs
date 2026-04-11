using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models
{
    public class UpdateViewModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
    }
}
