using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
