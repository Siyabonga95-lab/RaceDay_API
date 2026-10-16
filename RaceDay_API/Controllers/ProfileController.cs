using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay_API.Data;
using RaceDay_API.Models;

// This is my ProfileController.cs file, which handles CRUD operations for user profiles in the RaceDay_API project.
// It uses ASP.NET Core MVC and Entity Framework Core to interact with the database.
// The controller provides endpoints for retrieving and updating user profiles.

namespace RaceDay_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: api/profile
        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var role = HttpContext.Session.GetString("Role");

                if (userId == null) return Unauthorized("You must be logged in.");

                if (role == "Participant")
                {
                    var participant = await this._context.Participants.FirstOrDefaultAsync(p => p.UserID == userId);
                    if (participant == null) return NotFound("Profile not found.");
                    return Ok(participant);
                }
                else if (role == "Organiser")
                {
                    var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                    if (organiser == null) return NotFound("Profile not found.");
                    return Ok(organiser);
                }

                return BadRequest("Invalid role.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/profile
        [HttpPut]
        public async Task<IActionResult> UpdateMyProfile([FromBody] dynamic updatedData)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var role = HttpContext.Session.GetString("Role");

                if (userId == null) return Unauthorized("You must be logged in.");

                if (role == "Participant")
                {
                    var participant = await this._context.Participants.FirstOrDefaultAsync(p => p.UserID == userId);
                    if (participant == null) return NotFound("Profile not found.");

                    participant.ParticipantName = updatedData.Name;
                    participant.ParticipantEmail = updatedData.Email;
                    participant.ParticipantPhone = updatedData.Phone;

                    await this._context.SaveChangesAsync();
                    return Ok(new { Message = "Profile updated successfully." });
                }
                else if (role == "Organiser")
                {
                    var organiser = await this._context.Organisers.FirstOrDefaultAsync(o => o.UserID == userId);
                    if (organiser == null) return NotFound("Profile not found.");

                    organiser.OrganiserName = updatedData.Name;
                    organiser.OrganiserEmail = updatedData.Email;
                    organiser.OrganiserPhone = updatedData.Phone;

                    await this._context.SaveChangesAsync();
                    return Ok(new { Message = "Profile updated successfully." });
                }

                return BadRequest("Invalid role.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}