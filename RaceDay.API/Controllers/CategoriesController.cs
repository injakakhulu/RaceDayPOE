
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Security;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound("Category not found.");

            return Ok(category);
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetCategoriesForEvent(int eventId)
        {
            var categories = await _context.Categories
                .Where(c => c.EventId == eventId)
                .ToListAsync();

            return Ok(categories);
        }

        [HttpPost]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var eventItem = await _context.Events.FindAsync(category.EventId);

            if (eventItem == null)
                return NotFound("Event not found.");

            if (eventItem.OrganiserId != userId)
                return Forbid();

            category.CategoryId = 0;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.CategoryId },
                category);
        }

        [HttpPut("{id}")]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> UpdateCategory(
            int id, Category updatedCategory)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound("Category not found.");

            var eventItem = await _context.Events.FindAsync(category.EventId);
            var userId = HttpContext.Session.GetInt32("UserId");

            if (eventItem == null || eventItem.OrganiserId != userId)
                return Forbid();

            if (updatedCategory.EventId != category.EventId)
                return BadRequest("A category cannot be moved to another event.");

            category.CategoryName = updatedCategory.CategoryName;
            category.CategoryType = updatedCategory.CategoryType;
            category.Distance = updatedCategory.Distance;
            category.MinimumAge = updatedCategory.MinimumAge;
            category.MaximumAge = updatedCategory.MaximumAge;

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("{id}")]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound("Category not found.");

            var eventItem = await _context.Events.FindAsync(category.EventId);
            var userId = HttpContext.Session.GetInt32("UserId");

            if (eventItem == null || eventItem.OrganiserId != userId)
                return Forbid();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
