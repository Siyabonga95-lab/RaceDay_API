using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay_API.Data;
using RaceDay_API.Models;

// This is my CategoriesController.cs file, which handles CRUD operations for categories in the RaceDay_API project.
// It uses ASP.NET Core MVC and Entity Framework Core to interact with the database.

namespace RaceDay_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: api/categories/event/{eventId}
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetCategoriesForEvent(int eventId)
        {
            try
            {
                var categories = await this._context.Categories
                    .Where(c => c.EventID == eventId)
                    .ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/categories (Organiser Only)
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category newCategory)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Organiser" || userId == null) return StatusCode(403, "Only Organisers can create categories.");

                // Verify the organiser owns the event
                var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                var eventExists = await this._context.Events.AnyAsync(e => e.EventID == newCategory.EventID && e.OrganiserID == organiser.OrganiserID);

                if (!eventExists) return StatusCode(403, "You do not own this event.");

                this._context.Categories.Add(newCategory);
                await this._context.SaveChangesAsync();

                return Ok(new { Message = "Category created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/categories/{id} (Organiser Only)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Organiser" || userId == null) return StatusCode(403, "Only Organisers can update categories.");

                var existingCategory = await this._context.Categories.FindAsync(id);
                if (existingCategory == null) return NotFound("Category not found.");

                // Check ownership via the Event
                var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                var eventOwner = await this._context.Events.FirstOrDefaultAsync(e => e.EventID == existingCategory.EventID);

                if (eventOwner.OrganiserID != organiser.OrganiserID) return StatusCode(403, "You do not own this event.");

                existingCategory.CategoryAge = updatedCategory.CategoryAge;
                existingCategory.CategoryDistance = updatedCategory.CategoryDistance;

                await this._context.SaveChangesAsync();
                return Ok(new { Message = "Category updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/categories/{id} (Organiser Only)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Organiser" || userId == null) return StatusCode(403, "Only Organisers can delete categories.");

                var existingCategory = await this._context.Categories.FindAsync(id);
                if (existingCategory == null) return NotFound("Category not found.");

                var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                var eventOwner = await this._context.Events.FirstOrDefaultAsync(e => e.EventID == existingCategory.EventID);

                if (eventOwner.OrganiserID != organiser.OrganiserID) return StatusCode(403, "You do not own this event.");

                this._context.Categories.Remove(existingCategory);
                await this._context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}