using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class NoteGeneral
    {
        public List<EstudentListView> estudentListViews { get; set; }
        public List<NoteListView> noteListViews { get; set; }
        public List<CourseListView> courseListViews { get; set; }
        public List<SubjectListView> subjectListViews { get; set; }
    }
}