using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class CourseGeneral
    {
        public List<TeacherListView> TeacherListViews { get; set; }
        public List<CourseListView> CourseListViews { get; set; }
    }
}