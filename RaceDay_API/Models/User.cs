using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; } 
        public string PasswordHash { get; set; } 
        public string Role { get; set; }  // "Organiser" or "Participant"
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Participant? Participant { get; set; }
        public Organiser? Organiser { get; set; }
    }
}