using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        [HttpPost("user/authenticate")]
        public IActionResult StartDemo()
        {
            // user authentication we recommend using JSON Web Token for this part if your server is stateless.
            // we will just put simple string as an example

            // User.Identity.Name  can also be used if you are running forms authentication/cookie based authentication.
            return Ok("user1");
        }
    }
}
