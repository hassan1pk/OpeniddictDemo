using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.Models
{
    public class UserRegistrationModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        // Password should not be less than 8 characters
        // Should have alteast 1 digit
        // Should have atleast 1 non alphanumeric characters
        // Should have atleast 1 uppercase character
        // Should have atleast 1 lowercase character
        [RegularExpression(@"^(?=.*\d)(?=.*\W)(?=.*[A-Z])(?=.*[a-z]).{8,}$", ErrorMessage ="Password must meet the following requirements:<br/>- atleast 8 characters in length<br/>- have alteast 1 digit<br/>- have altleast 1 special character (e.g., $,#,+,*,-,@,&,etc.,)<br/>- have alteast 1 uppercase letter<br/>- have atleast 1 lower case letter")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
