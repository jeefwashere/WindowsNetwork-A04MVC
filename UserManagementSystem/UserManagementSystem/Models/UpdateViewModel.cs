using System.ComponentModel.DataAnnotations;
/*
   * FILE : UpdateModel.cs
   * PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
   * DESCRIPTION :set up for updateModel
   * 
   */
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
