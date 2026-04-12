/*
   * FILE : ProfileModel.cs
   * PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
   * DESCRIPTION :set up for Profileview model
   * 
   */
namespace UserManagementSystem.Models
{

    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PostalCode {  get; set; }
    }
}
