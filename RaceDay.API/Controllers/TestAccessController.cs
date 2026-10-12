using Microsoft.AspNetCore.Mvc;
using RaceDay.API.Security;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestAccessController : ControllerBase
    {
        [HttpGet("participant")]
        [SessionAuthorize("Participant")]
        public IActionResult ParticipantAccess()
        {
            return Ok("Participant access granted.");
        }

        [HttpGet("organiser")]
        [SessionAuthorize("Organiser")]
        public IActionResult OrganiserAccess()
        {
            return Ok("Organiser access granted.");
        }
    }
}