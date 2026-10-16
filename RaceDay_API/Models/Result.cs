using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Result
    {
        [Key]
        public int ResultID { get; set; }
        public int EnrollmentID { get; set; }
        public TimeSpan Result_finish_time { get; set; }
        public string Participant_position { get; set; }

        [ForeignKey("EnrollmentID")]
        public EventEnrollment? Enrollment { get; set; }
    }
}
