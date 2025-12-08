using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.Models
{
	public class Student
	{
		[Key]
		public int StudentID { get; set; }
		[Required]
		[MaxLength(150)]
		public string StudentFirstName { get; set; }
		[Required]
		[MaxLength(150)]
		public string StudentLastName { get; set; }
	}
}
