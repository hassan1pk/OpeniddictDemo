using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AuthorizationServer.Models
{
    public class ExternalLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public ClaimsPrincipal? Principal { get; set; }
    }
}
