using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public int CategoryAge { get; set; }
        public string CategoryDistance { get; set; }

        [ForeignKey("EventID")]
        public Event? Event { get; set; }
    }
}
