using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.Models
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
		public int Fk_ClassroomID { get; set; }
		[ForeignKey("Fk_ClassroomID")]
		public Classroom Classroom { get; set; }

	}
}
