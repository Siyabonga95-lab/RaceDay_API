using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public int OrganiserID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        public string EventDistance { get; set; }
        public string EventType { get; set; }  // Running, Walking, Cycling
        public string? RouteStartPoint { get; set; }
        public string? RouteEndPoint { get; set; }
        public string? RouteMapUrl { get; set; }

        [ForeignKey("OrganiserID")]
        public Organiser? Organiser { get; set; }
    }
}
