using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay_API.Data;
using RaceDay_API.Models;

// This is my ResultsController.cs file, which handles CRUD operations for results in the RaceDay_API project.
// It uses ASP.NET Core MVC and Entity Framework Core to interact with the database.
// The controller provides endpoints for capturing results (Organiser only), viewing own results (Participant only), and viewing results for a specific event (Public).

namespace RaceDay_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResultsController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // POST: api/results (Organiser Only - capture result)
        [HttpPost]
        public async Task<IActionResult> CaptureResult([FromBody] Result newResult)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Organiser" || userId == null) return StatusCode(403, "Only Organisers can capture results.");

                // Verify the enrollment exists and belongs to an event owned by this organiser
                var enrollment = await this._context.EventEnrollments
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(e => e.EnrollmentID == newResult.EnrollmentID);

                if (enrollment == null) return NotFound("Enrollment not found.");

                var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                if (enrollment.Event.OrganiserID != organiser.OrganiserID) return StatusCode(403, "You do not own this event.");

                this._context.Results.Add(newResult);
                await this._context.SaveChangesAsync();

                return Ok(new { Message = "Result captured successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/results/participant (Participant Only - view own results)
        [HttpGet("participant")]
        public async Task<IActionResult> GetMyResults()
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Participant" || userId == null) return StatusCode(403, "Access denied.");

                var participant = await this._context.Participants.FirstOrDefaultAsync(p => p.UserID == userId);

                var results = await this._context.Results
                    .Include(r => r.Enrollment)
                        .ThenInclude(e => e.Event)
                    .Include(r => r.Enrollment)
                        .ThenInclude(e => e.Category)
                    .Where(r => r.Enrollment.ParticipantID == participant.ParticipantID)
                    .ToListAsync();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/results/event/{eventId} (Public)
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetResultsForEvent(int eventId)
        {
            try
            {
                var results = await this._context.Results
                    .Include(r => r.Enrollment)
                        .ThenInclude(e => e.Participant)
                    .Where(r => r.Enrollment.EventID == eventId)
                    .ToListAsync();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}