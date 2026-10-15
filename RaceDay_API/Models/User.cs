using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "Organiser" or "Participant"
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Participant? Participant { get; set; }
        public Organiser? Organiser { get; set; }
    }

    public class Participant
    {
        [Key]
        public int ParticipantID { get; set; }
        public int UserID { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string ParticipantEmail { get; set; } = string.Empty;
        public string ParticipantPhone { get; set; } = string.Empty;

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }

    public class Organiser
    {
        [Key]
        public int OrganiserID { get; set; }
        public int UserID { get; set; }
        public string OrganiserName { get; set; } = string.Empty;
        public string OrganiserEmail { get; set; } = string.Empty;
        public string OrganiserPhone { get; set; } = string.Empty;

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }

    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public int OrganiserID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; } = string.Empty;
        public string EventDistance { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty; // Running, Walking, Cycling
        public string? RouteStartPoint { get; set; }
        public string? RouteEndPoint { get; set; }
        public string? RouteMapUrl { get; set; }

        [ForeignKey("OrganiserID")]
        public Organiser? Organiser { get; set; }
    }

    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public int CategoryAge { get; set; }
        public string CategoryDistance { get; set; } = string.Empty;

        [ForeignKey("EventID")]
        public Event? Event { get; set; }
    }

    public class EventEnrollment
    {
        [Key]
        public int EnrollmentID { get; set; }
        public int EventID { get; set; }
        public int ParticipantID { get; set; }
        public int CategoryID { get; set; }

        [ForeignKey("EventID")]
        public Event? Event { get; set; }
        [ForeignKey("ParticipantID")]
        public Participant? Participant { get; set; }
        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }
    }

    public class Result
    {
        [Key]
        public int ResultID { get; set; }
        public int EnrollmentID { get; set; }
        public TimeSpan Result_finish_time { get; set; }
        public string Participant_position { get; set; } = string.Empty;

        [ForeignKey("EnrollmentID")]
        public EventEnrollment? Enrollment { get; set; }
    }

    // DTOs for Login/Register
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}