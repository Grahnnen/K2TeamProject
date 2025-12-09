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
		public int FkCourseID { get; set; }
		[ForeignKey("FkCourseID")]
		public Course Course { get; set; }
		public int FkClassroomID { get; set; }
		[ForeignKey("FkClassroomID")]
		public Classroom Classroom { get; set; }

	}
}
