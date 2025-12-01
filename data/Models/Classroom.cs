using System.ComponentModel.DataAnnotations;

namespace SchoolClient.Models
{
    public class Classroom
    {
        [Key]
        public int ClassroomId { get; set; }
        [Required]
        [MaxLength(150)]
        public string ClassroomName { get; set; }
    }
}
