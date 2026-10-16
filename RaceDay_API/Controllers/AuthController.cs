using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay_API.Data;
using RaceDay_API.Models;
using RaceDay_API.DTOs;
using System.Security.Cryptography;
using System.Text;

//This is my AuthController.cs file, which handles user registration and login for the RaceDay_API project.
//It uses ASP.NET Core MVC and Entity Framework Core to interact with the database.
//The controller provides endpoints for user registration and login, including password hashing for security.

namespace RaceDay_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                if (request.Role != "Organiser" && request.Role != "Participant")
                {
                    return BadRequest("Role must be either 'Organiser' or 'Participant'.");
                }

                var existingUser = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
                if (existingUser != null)
                {
                    return Conflict("Username already exists.");
                }

                string passwordHash = HashPassword(request.Password);

                var newUser = new User
                {
                    UserName = request.Username,
                    PasswordHash = passwordHash,
                    Role = request.Role,
                    CreatedAt = DateTime.Now
                };

                this._context.Users.Add(newUser);
                await this._context.SaveChangesAsync();

                if (request.Role == "Participant")
                {
                    var participant = new Participant
                    {
                        UserID = newUser.UserID,
                        ParticipantName = request.Name,
                        ParticipantEmail = request.Email,
                        ParticipantPhone = request.Phone
                    };
                    this._context.Participants.Add(participant);
                }
                else if (request.Role == "Organiser")
                {
                    var organiser = new Organiser
                    {
                        UserID = newUser.UserID,
                        OrganiserName = request.Name,
                        OrganiserEmail = request.Email,
                        OrganiserPhone = request.Phone
                    };
                    this._context.Organisers.Add(organiser);
                }

                await this._context.SaveChangesAsync();

                return Ok(new { Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                string inputHash = HashPassword(request.Password);
                if (user.PasswordHash != inputHash)
                {
                    return Unauthorized("Invalid username or password.");
                }

                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("Role", user.Role);

                return Ok(new
                {
                    Message = "Login successful",
                    UserID = user.UserID,
                    Role = user.Role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}