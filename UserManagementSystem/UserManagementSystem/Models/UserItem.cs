namespace UserManagementSystem.Models
/*
* FILE : UserItem.cs
* PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
* DESCRIPTION :set up for UserItemModel
* 
*/
{
    public class UserItem
    {
        public int UserItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int OwnerID { get; set; }
        public User? Owner { get; set; } = null;
    }
}
