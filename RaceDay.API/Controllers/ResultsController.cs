
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Security;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("mine")]
        [SessionAuthorize("Participant")]
        public async Task<IActionResult> GetMyResults()
        {
            var participantId = HttpContext.Session.GetInt32("UserId");

            var results = await _context.Results
                .Include(r => r.Enrolment)
                .Where(r => r.Enrolment.ParticipantId == participantId)
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventResults(int eventId)
        {
            var eventItem = await _context.Events.FindAsync(eventId);

            if (eventItem == null)
                return NotFound("Event not found.");

            var results = await _context.Results
                .Include(r => r.Enrolment)
                .Where(r => r.Enrolment.EventId == eventId)
                .OrderBy(r => r.Position)
                .ToListAsync();

            return Ok(results);
        }

        [HttpPost]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> CreateResult(Result result)
        {
            var enrolment = await _context.Enrolments
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.EnrolmentId == result.EnrolmentId);

            if (enrolment == null)
                return NotFound("Enrolment not found.");

            var userId = HttpContext.Session.GetInt32("UserId");

            if (enrolment.Event == null || enrolment.Event.OrganiserId != userId)
                return Forbid();

            var alreadyHasResult = await _context.Results
                .AnyAsync(r => r.EnrolmentId == result.EnrolmentId);

            if (alreadyHasResult)
                return Conflict("A result already exists for this enrolment.");

            result.ResultId = 0;
            result.CapturedAt = DateTime.UtcNow;

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEventResults),
                new { eventId = enrolment.EventId },
                result);
        }

        [HttpPut("{id}")]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> UpdateResult(int id, Result updatedResult)
        {
            var result = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.Event)
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (result == null)
                return NotFound("Result not found.");

            var userId = HttpContext.Session.GetInt32("UserId");

            if (result.Enrolment.Event == null ||
                result.Enrolment.Event.OrganiserId != userId)
                return Forbid();

            result.FinishTime = updatedResult.FinishTime;
            result.Position = updatedResult.Position;
            result.ResultStatus = updatedResult.ResultStatus;
            result.CapturedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            var result = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.Event)
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (result == null)
                return NotFound("Result not found.");

            var userId = HttpContext.Session.GetInt32("UserId");

            if (result.Enrolment.Event == null ||
                result.Enrolment.Event.OrganiserId != userId)
                return Forbid();

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
