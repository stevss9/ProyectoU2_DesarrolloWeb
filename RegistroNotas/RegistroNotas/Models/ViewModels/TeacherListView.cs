using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class TeacherListView
    {
        public int id { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string specialty { get; set; }
        public string email { get; set; }
    }
}