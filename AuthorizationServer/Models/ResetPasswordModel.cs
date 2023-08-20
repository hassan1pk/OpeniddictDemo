using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*\W)(?=.*[A-Z])(?=.*[a-z]).{8,}$", ErrorMessage = "Password must meet the following requirements:<br/>- atleast 8 characters in length<br/>- have alteast 1 digit<br/>- have altleast 1 special character (e.g., $,#,+,*,-,@,&,etc.,)<br/>- have alteast 1 uppercase letter<br/>- have atleast 1 lower case letter")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
