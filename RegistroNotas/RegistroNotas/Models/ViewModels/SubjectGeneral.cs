using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class SubjectGeneral
    {
        public List<TeacherListView> teacherListViews { get; set; }
        public List<SubjectListView> subjectListViews { get; set; }
    }
}