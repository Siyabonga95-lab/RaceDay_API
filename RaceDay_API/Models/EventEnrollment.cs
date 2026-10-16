using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
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
}
