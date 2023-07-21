using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class SubjectListView
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public int? id_teacher { get; set; }
    }
}