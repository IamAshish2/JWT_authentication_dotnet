using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JWT_AUTHENTICATION.Models.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
