using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JWT_AUTHENTICATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {

        [Authorize]
        [HttpGet("data")]
        public IActionResult Index()
        {
            // decoding claims
            string id = HttpContext.User.FindFirstValue("id")!;
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Email)!;
            string username = HttpContext.User.FindFirstValue(ClaimTypes.Name)!;
            return Ok(new { id, email, username });
        }
    }
}
