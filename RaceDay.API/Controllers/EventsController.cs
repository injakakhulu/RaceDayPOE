
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Security;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound("Event not found.");

            return Ok(eventItem);
        }

        [HttpPost]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> CreateEvent(Event eventItem)
        {
            eventItem.EventId = 0;
            eventItem.OrganiserId =
                HttpContext.Session.GetInt32("UserId")!.Value;
            eventItem.CreatedAt = DateTime.UtcNow;

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = eventItem.EventId },
                eventItem);
        }

        [HttpPut("{id}")]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> UpdateEvent(
            int id, Event updatedEvent)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound("Event not found.");

            var userId = HttpContext.Session.GetInt32("UserId");

            if (eventItem.OrganiserId != userId)
                return Forbid();

            eventItem.EventName = updatedEvent.EventName;
            eventItem.Description = updatedEvent.Description;
            eventItem.EventDate = updatedEvent.EventDate;
            eventItem.Location = updatedEvent.Location;
            eventItem.Distance = updatedEvent.Distance;
            eventItem.EventType = updatedEvent.EventType;

            await _context.SaveChangesAsync();

            return Ok(eventItem);
        }

        [HttpDelete("{id}")]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound("Event not found.");

            var userId = HttpContext.Session.GetInt32("UserId");

            if (eventItem.OrganiserId != userId)
                return Forbid();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// ////////////
        /// </summary>
       
    }
}
