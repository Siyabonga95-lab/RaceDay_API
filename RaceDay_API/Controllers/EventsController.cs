using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay_API.Data;
using RaceDay_API.Models;

namespace RaceDay_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: api/events (Public)
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await this._context.Events
                    .Include(e => e.Organiser)
                    .ToListAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/events/{id} (Public)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                var ev = await this._context.Events
                    .Include(e => e.Organiser)
                    .FirstOrDefaultAsync(e => e.EventID == id);

                if (ev == null) return NotFound("Event not found.");
                return Ok(ev);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/events (Organiser Only)
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Organiser" || userId == null)
                {
                    return StatusCode(403, "Only Organisers can create events.");
                }

                var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                if (organiser == null) return BadRequest("Organiser profile not found.");

                newEvent.OrganiserID = organiser.OrganiserID;
                this._context.Events.Add(newEvent);
                await this._context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEventById), new { id = newEvent.EventID }, newEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/events/{id} (Organiser Only)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Organiser" || userId == null) return StatusCode(403, "Only Organisers can update events.");

                var existingEvent = await this._context.Events.FindAsync(id);
                if (existingEvent == null) return NotFound("Event not found.");

                var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                if (existingEvent.OrganiserID != organiser.OrganiserID) return StatusCode(403, "You do not own this event.");

                existingEvent.EventName = updatedEvent.EventName;
                existingEvent.EventDescription = updatedEvent.EventDescription;
                existingEvent.EventDate = updatedEvent.EventDate;
                existingEvent.EventLocation = updatedEvent.EventLocation;
                existingEvent.EventDistance = updatedEvent.EventDistance;
                existingEvent.EventType = updatedEvent.EventType;
                existingEvent.RouteStartPoint = updatedEvent.RouteStartPoint;
                existingEvent.RouteEndPoint = updatedEvent.RouteEndPoint;
                existingEvent.RouteMapUrl = updatedEvent.RouteMapUrl;

                await this._context.SaveChangesAsync();
                return Ok(existingEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/events/{id} (Organiser Only)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Organiser" || userId == null) return StatusCode(403, "Only Organisers can delete events.");

                var existingEvent = await this._context.Events.FindAsync(id);
                if (existingEvent == null) return NotFound("Event not found.");

                var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                if (existingEvent.OrganiserID != organiser.OrganiserID) return StatusCode(403, "You do not own this event.");

                this._context.Events.Remove(existingEvent);
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