using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthController : ControllerBase
    {
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody]object registrationData)
        {
            if(registrationData == null)
            {
                return BadRequest("Registration data must not be null");
            }

            return Created();
        }
    }
}
