using System.ComponentModel.DataAnnotations;

namespace K2TeamProject.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        [Required]
        [MaxLength(150)]
        public string StudentName { get; set; }
    }
}
