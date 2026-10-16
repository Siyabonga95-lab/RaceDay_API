using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Organiser
    {
        [Key]
        public int OrganiserID { get; set; }
        public int UserID { get; set; }
        public string OrganiserName { get; set; }
        public string OrganiserEmail { get; set; }
        public string OrganiserPhone { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
