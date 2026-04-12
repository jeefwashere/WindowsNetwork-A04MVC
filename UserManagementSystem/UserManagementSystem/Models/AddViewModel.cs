using System.ComponentModel.DataAnnotations;
/*
   * FILE : AddViewModel.cs
   * PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
   * DESCRIPTION :set up for add item page, the user can add item to the inventory
   * 
   */
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
