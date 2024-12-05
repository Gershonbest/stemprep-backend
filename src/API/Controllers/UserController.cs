using Application.Auth.Commands;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : Controller
    {
        [HttpPost("user/gettype")]
        public async Task<IActionResult> getType()
        {
            return Ok();
        }
    }
}
