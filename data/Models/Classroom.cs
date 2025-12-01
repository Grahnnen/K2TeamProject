using System.ComponentModel.DataAnnotations;

namespace K2TeamProject.Models
{
    public class Classroom
    {
        [Key]
        public int ClassroomID { get; set; }
        [Required]
        [MaxLength(150)]
        public string ClassroomName { get; set; }
    }
}
