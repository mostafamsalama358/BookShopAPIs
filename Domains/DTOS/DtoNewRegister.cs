using System.ComponentModel.DataAnnotations;



namespace Domains.DTOS
{
    //public enum UserRole
    //{
    //    Admin,
    //    Customer,
    //    DataEntry,
    //    Writer

    ////    //    public static readonly List<string> Roles = new List<string>
    ////    //{
    ////    //    "Admin",
    ////    //    "Customer",
    ////    //    "DataEntry",
    ////    //    "Writer"
    ////    //};
    //}
    public class DtoNewRegister
    {
        [Required]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please Enter E-Mail Valid")]
        [EmailAddress]
        public string Email { get; set; }   
        [Required(ErrorMessage = "Please Enter Correct Passowrd")]
        public string Password { get; set; }
        public string Role{ get; set; } // Use enum for role selection
    }
}
