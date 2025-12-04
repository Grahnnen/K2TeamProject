using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K2TeamProject.Models
{
    public class Grade
    {
        [Key]
        public int GradeID { get; set; }
        [Required]
        [MaxLength(150)]
        public DateOnly GradedDate { get; set; }
        public string GradeScale { get; set; }

        //Foreign Keys
        public int Fk_StudentID { get; set; }
        [ForeignKey("Fk_StudentID")]
        public Student Student { get; set; }

        public int Fk_TeacherID { get; set; }
        [ForeignKey("Fk_TeacherID")]
        public Teacher Teacher { get; set; }
    }
}
