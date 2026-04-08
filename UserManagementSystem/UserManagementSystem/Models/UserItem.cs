namespace UserManagementSystem.Models
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
