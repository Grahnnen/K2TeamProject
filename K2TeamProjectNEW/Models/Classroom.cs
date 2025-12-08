using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.Models
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
