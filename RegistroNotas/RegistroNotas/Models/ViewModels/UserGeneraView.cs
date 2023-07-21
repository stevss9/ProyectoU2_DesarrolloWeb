using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class UserGeneraView
    {
        public List<EstudentListView> EstudentListViews { get; set; }
        public List<TeacherListView> TeacherListViews { get; set; }

        public List<UserListView> UserListViews { get; set; }
    }
}