using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.Models
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
		public int FkStudentID { get; set; }
		[ForeignKey("FkStudentID")]
		public Student Student { get; set; }


		public int FkTeacherID { get; set; }
		[ForeignKey("FkTeacherID")]
		public Teacher Teacher { get; set; }
	}
}
