using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay_API.Data;
using RaceDay_API.Models;

namespace RaceDay_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // POST: api/enrollments (Participant Only)
        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EventEnrollment enrollment)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Participant" || userId == null)
                {
                    return StatusCode(403, "Only Participants can enroll.");
                }

                var participant = await this._context.Participants.FirstOrDefaultAsync(p => p.UserID == userId);
                if (participant == null) return BadRequest("Participant profile not found.");

                var existing = await this._context.EventEnrollments
                    .FirstOrDefaultAsync(e => e.ParticipantID == participant.ParticipantID && e.CategoryID == enrollment.CategoryID);

                if (existing != null) return Conflict("You are already enrolled in this category.");

                enrollment.ParticipantID = participant.ParticipantID;
                this._context.EventEnrollments.Add(enrollment);
                await this._context.SaveChangesAsync();

                return Ok(new { Message = "Enrolled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/enrollments/participant (Participant Only)
        [HttpGet("participant")]
        public async Task<IActionResult> GetMyEnrollments()
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var userId = HttpContext.Session.GetInt32("UserID");

                if (role != "Participant" || userId == null) return StatusCode(403, "Access denied.");

                var participant = await this._context.Participants.FirstOrDefaultAsync(p => p.UserID == userId);
                if (participant == null) return BadRequest("Participant profile not found.");

                var enrollments = await this._context.EventEnrollments
                    .Include(e => e.Event)
                    .Include(e => e.Category)
                    .Where(e => e.ParticipantID == participant.ParticipantID)
                    .ToListAsync();

                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}