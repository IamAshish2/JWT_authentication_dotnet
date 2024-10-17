using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace JWT_AUTHENTICATION.Models.Requests
{
    public class LoginRequest
    {
        //[Key]
        //public int LoginId { get; set; } 
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
