using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RegistroNotas.Models.ViewModels
{
    public class UserListView
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public int? id_role { get; set; }
        public int? id_student { get; set; }
        public int? id_teacher { get; set; }
    }
}