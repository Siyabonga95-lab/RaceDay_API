using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantID { get; set; }
        public int UserID { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantEmail { get; set; }
        public string ParticipantPhone { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
