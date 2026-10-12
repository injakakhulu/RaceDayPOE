
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Security;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("mine")]
        [SessionAuthorize("Participant")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var participantId = HttpContext.Session.GetInt32("UserId");

            var enrolments = await _context.Enrolments
                .Where(e => e.ParticipantId == participantId)
                .ToListAsync();

            return Ok(enrolments);
        }

        [HttpGet("{id}")]
        [SessionAuthorize("Participant", "Organiser")]
        public async Task<IActionResult> GetEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);

            if (enrolment == null)
                return NotFound("Enrolment not found.");

            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            if (role == "Participant" && enrolment.ParticipantId != userId)
                return Forbid();

            if (role == "Organiser")
            {
                var eventItem = await _context.Events
                    .FindAsync(enrolment.EventId);

                if (eventItem == null || eventItem.OrganiserId != userId)
                    return Forbid();
            }

            return Ok(enrolment);
        }

        [HttpPost]
        [SessionAuthorize("Participant")]
        public async Task<IActionResult> CreateEnrolment(Enrolment enrolment)
        {
            var participantId = HttpContext.Session.GetInt32("UserId");

            var eventItem = await _context.Events
                .FindAsync(enrolment.EventId);

            if (eventItem == null)
                return NotFound("Event not found.");

            var category = await _context.Categories
                .FindAsync(enrolment.CategoryId);

            if (category == null || category.EventId != enrolment.EventId)
                return BadRequest("The category does not belong to this event.");

            var alreadyEnrolled = await _context.Enrolments.AnyAsync(e =>
                e.ParticipantId == participantId &&
                e.EventId == enrolment.EventId);

            if (alreadyEnrolled)
                return Conflict("You are already enrolled in this event.");

            enrolment.EnrolmentId = 0;
            enrolment.ParticipantId = participantId!.Value;
            enrolment.EnrolmentDate = DateTime.UtcNow;
            enrolment.Status = "Registered";

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEnrolment),
                new { id = enrolment.EnrolmentId },
                enrolment);
        }

        [HttpDelete("{id}")]
        [SessionAuthorize("Participant")]
        public async Task<IActionResult> CancelEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);

            if (enrolment == null)
                return NotFound("Enrolment not found.");

            var participantId = HttpContext.Session.GetInt32("UserId");

            if (enrolment.ParticipantId != participantId)
                return Forbid();

            _context.Enrolments.Remove(enrolment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /////////////////////////////////
        ///
   

    }
}
