using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.Models
{
	public class Enrollment
	{
		[Key]
		public int EnrollmentID { get; set; }
		[Required]
		[MaxLength(150)]
		public DateOnly RegistrationDate { get; set; }

		//Foreign Keys
		public int FkStudentID { get; set; }
		[ForeignKey("FkStudentID")]
		public Student Student { get; set; }

		public int FkCourseID { get; set; }
		[ForeignKey("FkCourseID")]
		public Course Course { get; set; }

		public int FkGradeID { get; set; }
		[ForeignKey("FkGradeID")]
		public Grade Grade { get; set; }

	}
}
