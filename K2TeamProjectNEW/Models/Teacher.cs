using System;
using System.Collections.Generic;

namespace K2TeamProjectNEW.Models;

public partial class Teacher
{
    public int TeacherID { get; set; }

    public string TeacherName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
