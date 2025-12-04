using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K2TeamProject.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }
        [Required]
        [MaxLength(150)]
        public DateOnly RegistrationDate { get; set; }

        //Foreign Keys
        public int Fk_StudentID { get; set; }
        [ForeignKey("Fk_StudentID")]
        public Student Student { get; set; }

        public int Fk_CourseID { get; set; }
        [ForeignKey("Fk_CourseID")]
        public Course Course { get; set; }

        public int Fk_GradeID { get; set; }
        [ForeignKey("Fk_GradeID")]
        public Grade Grade { get; set; }
    }
}
