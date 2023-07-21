using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class CourseListView
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? id_teacher { get; set; }
    }
}