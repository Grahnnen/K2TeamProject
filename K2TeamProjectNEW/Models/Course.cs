using System;
using System.Collections.Generic;

namespace K2TeamProjectNEW.Models;

public partial class Course
{
    public int CourseID { get; set; }

    public string CourseName { get; set; } = null!;

    public int? FkTeacherID { get; set; }

    public DateOnly? CourseStartDate { get; set; }

    public DateOnly? CourseEndDate { get; set; }

    public virtual Teacher? FkTeacher { get; set; }

}