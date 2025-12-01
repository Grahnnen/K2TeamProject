using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolClient.Models
{
    public class Scheduling
    {
        [Key]
        public int SchedulingID { get; set; }
        [Required]
        [MaxLength(150)]
        public DateTime SchedulingStartDateTime { get; set; }
        public DateTime SchedulingEndDateTime { get; set; }

        //Foreign Keys
        public int Fk_CourseID { get; set; }
        [ForeignKey("Fk_CourseID")]
        public Course Course { get; set; }
    }
}
