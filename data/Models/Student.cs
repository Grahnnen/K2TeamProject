using System.ComponentModel.DataAnnotations;

namespace SchoolClient.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        [MaxLength(150)]
        public string StudentName { get; set; }
    }
}
